using RedditSharp;
using System;

namespace RedditCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            GetPosts();
        }

        private static void GetPosts()
        {
            var webAgent = new BotWebAgent("BotUsername", "BotPass", "ClientID", "ClientSecret", "RedirectUri");
            //This will check if the access token is about to expire before each request and automatically request a new one for you
            //"false" means that it will NOT load the logged in user profile so reddit.User will be null
            var reddit = new Reddit(webAgent, false);
            var subreddit = reddit.GetSubreddit("/r/example");
            subreddit.Subscribe();
            foreach (var post in subreddit.New.Take(25))
            {
                if (post.Title == "What is my karma?")
                {
                    // Note: This is an example. Bots are not permitted to cast votes automatically.
                    post.Upvote();
                    var comment = post.Comment(string.Format("You have {0} link karma!", post.Author.LinkKarma));
                    comment.Distinguish(DistinguishType.Moderator);
                }
            }
        }
    }
}