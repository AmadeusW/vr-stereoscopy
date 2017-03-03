using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RedditSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StereoscopyVR.RedditCrawler
{
    class Program
    {
        const string SaveLocation = "out";
        const string DownloadLocation = "drop";
        const string PostsFile = "posts.json";
        static public IConfigurationRoot Configuration { get; set; }
        static void Main(string[] args)
        {
            Directory.CreateDirectory(SaveLocation);
            Directory.CreateDirectory(DownloadLocation);

            Configure();
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            Console.WriteLine("Do you want to download new posts? (y, enter)");
            IEnumerable<CrossViewPost> posts = null;
            if (Console.ReadLine().ToLowerInvariant() == "y")
            {
                Console.WriteLine("Downloading data from Reddit...");
                posts = await GetPosts();
                Console.WriteLine("Writing data to disk...");
                using (StreamWriter file = File.CreateText(Path.Combine(SaveLocation, PostsFile)))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(file, posts);
                }
            }
            else
            {
                Console.WriteLine("Reading data from disk...");
                using (StreamReader file = File.OpenText(Path.Combine(SaveLocation, PostsFile)))
                {
                    var serializer = new JsonSerializer();
                    posts = (IEnumerable<CrossViewPost>)serializer.Deserialize(file, typeof(IEnumerable<CrossViewPost>));
                }
            }

            Console.WriteLine("Fetching image URLs");
            GetImageUrls(posts);

            Console.WriteLine("Do you want to process images? (y, enter)");
            if (Console.ReadLine().ToLowerInvariant() == "y")
            {
                await DoWork(posts);
            }

            Console.WriteLine("All done. Hit Enter to exit.");
            Console.ReadLine();
        }

        private static void GetImageUrls(IEnumerable<CrossViewPost> posts)
        {
            foreach (var post in posts)
            {
                post.TryGetImageUrl();
                if (post.ImageUrl == null)
                {
                    Console.WriteLine($"Not supported domain {post.Url.Host}: [{post.Score}] {post.Title}");
                }
                else
                {
                    Console.WriteLine($"OK: [{post.Score}] {post.Title}");
                }
            }
        }

        private static async Task GetAndSavePosts()
        {
            var posts = await GetPosts();

        }

        private static async Task DoWork(IEnumerable<CrossViewPost> posts)
        {
            foreach (var post in posts.Where(n => n.ImageUrl != null))
            {
                var filePath = Path.Combine(DownloadLocation, post.Link + ".img");
                await DownloadAsync(post, filePath);
                ImageApp.Program.ProcessFile(filePath);
            }
        }

        private static async Task DownloadAsync(CrossViewPost post, string filePath)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, post.ImageUrl);
                using (var response = await client.SendAsync(request))
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    var dataStream = await response.Content.ReadAsStreamAsync();
                    await dataStream.CopyToAsync(fileStream);
                }
            }
        }

        private static void Configure()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(@"D:\vrcv.json");
            Configuration = builder.Build();
        }

        private static async Task<IEnumerable<CrossViewPost>> GetPosts()
        {
            var webAgent = new BotWebAgent(Configuration["username"], Configuration["password"], Configuration["token"], Configuration["secret"], Configuration["url"]);
            //This will check if the access token is about to expire before each request and automatically request a new one for you
            //"false" means that it will NOT load the logged in user profile so reddit.User will be null
            var reddit = new Reddit(webAgent, false);
            await reddit.InitOrUpdateUserAsync();
            var authenticated = reddit.User != null;
            if (!authenticated)
                Console.WriteLine("Invalid token");

            var subreddit = await reddit.GetSubredditAsync("/r/crossview");
            var posts = new List<CrossViewPost>();
            await subreddit.GetTop(RedditSharp.Things.FromTime.Week).Take(50).ForEachAsync(post => {
                var data = new CrossViewPost(post.Url, post.Title, post.Shortlink, post.Score, post.CreatedUTC);
                Console.WriteLine(post.Title);
                posts.Add(data);
            });
            return posts;
        }
    }
}