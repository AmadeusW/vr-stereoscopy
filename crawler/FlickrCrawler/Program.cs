using System;
using StereoscopyVR.CoreData.Endpoints;
using System.Collections.Generic;
using StereoscopyVR.CoreData;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using StereoscopyVR.CoreData.Data;
using System.Linq;

namespace FlickrCrawler
{
    class Program
    {
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
            Console.WriteLine("Fetching image URLs...");

            // 3D Insects in Flight 2015 by Franz
            var album = await flickr.ScrapeAlbum("24851601@N02", "72157648972433403", "fotoopa_hs");

            Console.WriteLine("Writing data to disk...");
            Directory.CreateDirectory(DownloadLocation);
            using (StreamWriter file = File.CreateText(Path.Combine(DownloadLocation, PostsFile)))
            {
                var serializer = new JsonSerializer();
                var collection = new SceneCollection() { scenes = album };
                serializer.Serialize(file, collection);
            }
        }
    }
}