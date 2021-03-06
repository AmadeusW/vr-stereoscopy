﻿using Newtonsoft.Json;
using RedditSharp;
using StereoscopyVR.CoreData;
using StereoscopyVR.CoreData.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StereoscopyVR.RedditCrawler
{
    class Program
    {
        // TODO: Consider using F#

        const string SaveLocation = "out";
        const string DownloadLocation = "drop";
        const string PostsFile = "posts.json";
        
        static void Main(string[] args)
        {
            Directory.CreateDirectory(SaveLocation);
            Directory.CreateDirectory(DownloadLocation);

            CoreData.Configuration.Initialize();
            MainAsync().Wait(System.Threading.Timeout.Infinite);
        }

        private static async Task MainAsync()
        {
            Console.WriteLine("Do you want to download new posts? (y, enter)");
            IEnumerable<StereoImage> posts = null;
            if (Console.ReadLine().ToLowerInvariant() == "y")
            {
                Console.WriteLine("Downloading data from Reddit...");
                posts = await GetPosts();

                Console.WriteLine("Fetching image URLs...");
                posts = await GetImageUrls(posts);

                Console.WriteLine("Writing data to disk...");
                using (StreamWriter file = File.CreateText(Path.Combine(DownloadLocation, PostsFile)))
                {
                    var serializer = new JsonSerializer();
                    var collection = new SceneCollection() { scenes = posts };
                    serializer.Serialize(file, collection);
                }
            }
            else
            {
                Console.WriteLine("Reading data from disk...");
                using (StreamReader file = File.OpenText(Path.Combine(DownloadLocation, PostsFile)))
                {
                    var serializer = new JsonSerializer();
                    var collection = (SceneCollection)serializer.Deserialize(file, typeof(SceneCollection));
                    posts = collection.scenes;
                }
            }

            //Console.WriteLine("Do you want to process images? (y, enter)");
            //if (Console.ReadLine().ToLowerInvariant() == "y")
            {
                var processedPosts = await DoWork(posts);
                using (StreamWriter file = File.CreateText(Path.Combine(SaveLocation, PostsFile)))
                {
                    var serializer = new JsonSerializer();
                    var collection = new SceneCollection() { scenes = processedPosts };
                    serializer.Serialize(file, collection);
                }
            }

            Console.WriteLine("All done. Hit Enter to exit.");
            Console.ReadLine();
        }

        private static async Task<IEnumerable<StereoImage>> GetImageUrls(IEnumerable<StereoImage> posts)
        {
            var allPosts = new List<RedditCrossViewPost>();

            foreach (var post in posts.Select(n => (RedditCrossViewPost)n))
            {
                try
                {
                    var updatedPosts = await post.WithUpdatedImageUrl();
                    if (!updatedPosts.Any())
                    {
                        Console.WriteLine($"Not supported domain {post.Url.Host}: [{post.Score}] {post.Title}");
                    }

                    allPosts.AddRange(updatedPosts.Select(updatedPost =>
                    {
                        Console.WriteLine($"OK: [{updatedPost.Score}] {updatedPost.Title}");
                        return updatedPost;
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error at {post.Url.Host}: {ex}");
                }
            }
            return allPosts;
        }

        private static async Task GetAndSavePosts()
        {
            var posts = await GetPosts();

        }

        private static async Task<IEnumerable<StereoImage>> DoWork(IEnumerable<StereoImage> posts)
        {
            var processedPosts = new List<StereoImage>();
            foreach (var post in posts.Where(n => n.ImageUrl != null))
            {
                var name = post.ImageUrl.Segments.Last();
                Console.Write($"Processing {name}: ");
                var filePath = Path.Combine(DownloadLocation, name);
                if (File.Exists(filePath))
                {
                    Console.Write("Using previously downloaded file; ");
                }
                else
                {
                    try
                    {
                        Console.Write("Downloading; ");
                        await DownloadAsync(post, filePath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error downloading {post.ImageUrl}: {e.Message}");
                    }
                }
                try
                {
                    var imageProperties = ImageApp.Program.ProcessCrossViewFile(filePath);
                    post.W = imageProperties.Width;
                    post.H = imageProperties.Height;
                    // TODO: refactor to make data structures nice and immutable
                    processedPosts.Add(post);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error processing {filePath}: {e.Message}");
                }
            }
            return processedPosts;
        }

        private static async Task DownloadAsync(StereoImage post, string filePath)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, post.ImageUrl);
                using (var response = await client.SendAsync(request, new CancellationToken()))
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    var dataStream = await response.Content.ReadAsStreamAsync();
                    await dataStream.CopyToAsync(fileStream);
                }
            }
        }

        private static async Task<IEnumerable<RedditCrossViewPost>> GetPosts()
        {
            var webAgent = new BotWebAgent(Configuration.Instance["reddit-username"], Configuration.Instance["reddit-password"], Configuration.Instance["reddit-token"], Configuration.Instance["reddit-secret"], Configuration.Instance["reddit-url"]);
            //This will check if the access token is about to expire before each request and automatically request a new one for you
            //"false" means that it will NOT load the logged in user profile so reddit.User will be null
            var reddit = new Reddit(webAgent, false);
            await reddit.InitOrUpdateUserAsync();
            var authenticated = reddit.User != null;
            if (!authenticated)
                Console.WriteLine("Invalid token");

            var subreddit = await reddit.GetSubredditAsync("/r/crossview");
            var posts = new List<RedditCrossViewPost>();
            //await subreddit.GetTop(RedditSharp.Things.FromTime.All).Where(n => n.CreatedUTC.Year == 2016).Take(50).ForEachAsync(post => {
            //await subreddit.GetTop(RedditSharp.Things.FromTime.Month).Take(50).ForEachAsync(post => {
            await reddit.GetPostAsync(new Uri(@"https://www.reddit.com/r/CrossView/comments/1ujpnj/some_1920s_stereograms/?ref=share&ref_source=link")).ToAsyncEnumerable().ForEachAsync(post => {
                var data = new RedditCrossViewPost(post.Url, null, post.Title, post.Shortlink, post.Score, post.CreatedUTC);
                Console.WriteLine(post.Title);
                posts.Add(data);
            });
            return posts;
        }
    }
}