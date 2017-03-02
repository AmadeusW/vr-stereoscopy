using Microsoft.Extensions.Configuration;
using RedditSharp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RedditCrawler
{
    class Program
    {
        static public IConfigurationRoot Configuration { get; set; }
        static void Main(string[] args)
        {
            Configure();
            GetPosts().Wait();
        }

        private static void Configure()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile(@"D:\vrcv.json");
            Configuration = builder.Build();
        }

        private static async Task GetPosts()
        {
            var webAgent = new BotWebAgent(Configuration["username"], Configuration["password"], Configuration["token"], Configuration["secret"], Configuration["url"]);
            //This will check if the access token is about to expire before each request and automatically request a new one for you
            //"false" means that it will NOT load the logged in user profile so reddit.User will be null
            var reddit = new Reddit(webAgent, false);
            //var reddit = new Reddit("kHw4KMboW8wUEA");
            await reddit.InitOrUpdateUserAsync();
            var authenticated = reddit.User != null;
            if (!authenticated)
                Console.WriteLine("Invalid token");

            var subreddit = await reddit.GetSubredditAsync("/r/example");
            await subreddit.GetTop(RedditSharp.Things.FromTime.Month).Take(25).ForEachAsync(post => {
                Console.WriteLine(post.Title);
            });
        }
    }
}