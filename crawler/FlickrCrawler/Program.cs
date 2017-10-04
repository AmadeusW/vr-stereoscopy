using System;
using StereoscopyVR.CoreData.Endpoints;
using System.Collections.Generic;
using StereoscopyVR.CoreData;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using StereoscopyVR.CoreData.Data;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace StereoscopyVR.FlickrCrawler
{
    class Program
    {
        const string SaveLocation = "out";
        const string DownloadLocation = "drop";
        const string PostsFile = "posts.json";

        static void Main(string[] args)
        {
            Configuration.Initialize();
            try
            {
                MainAsync().Wait(System.Threading.Timeout.Infinite);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static async Task MainAsync()
        {
            var flickr = new Flickr();

            Console.WriteLine("Do you want to download new posts? (y, enter)");
            IEnumerable<StereoImage> posts = null;
            if (Console.ReadLine().ToLowerInvariant() == "y")
            {
                Console.WriteLine("Fetching image URLs...");

                // 3D Insects in Flight 2015 by Franz
                posts = await flickr.ScrapeAlbum("24851601@N02", "72157648972433403", "fotoopa_hs");

                Console.WriteLine("Writing data to disk...");
                Directory.CreateDirectory(DownloadLocation);
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

            Directory.CreateDirectory(SaveLocation);
            var processedPosts = await DoWork(posts);
            using (StreamWriter file = File.CreateText(Path.Combine(SaveLocation, PostsFile)))
            {
                var serializer = new JsonSerializer();
                var collection = new SceneCollection() { scenes = processedPosts };
                serializer.Serialize(file, collection);
            }

            Console.WriteLine("All done. Hit Enter to exit.");
            Console.ReadLine();
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
    }
}