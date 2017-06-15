using StereoscopyVR.ImageApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StereoscopyVR.RedditCrawler;

namespace MyImages
{
    class Program
    {
        const string SaveLocation = "out";
        const string PostsFile = "posts.json";

        static void Main(string[] args)
        {
            DoWork();            
        }

        private static void DoWork()
        {
            var processedPosts = new List<CrossViewPost>();
            var files = GetPairs(@"C:\Users\amadeusz\Pictures\Stereo\out\").ToArray();
            for (int i = 0; i < files.Length; i += 2)
            {
                var file1 = files[i];   // Right eye view
                var file2 = files[i+1]; // Left eye view
                var name = Path.GetFileNameWithoutExtension(file1);
                processedPosts.Add(ProcessPair(file1, file2, name));
            }
            
            using (StreamWriter file = File.CreateText(Path.Combine(SaveLocation, PostsFile)))
            {
                var serializer = new JsonSerializer();
                var collection = new SceneCollection() { scenes = processedPosts };
                serializer.Serialize(file, collection);
            }
        }

        private static IEnumerable<string> GetPairs(string directory)
        {
            return Directory.EnumerateFiles(directory).OrderBy(n => n);
        }

        private static CrossViewPost ProcessPair(string path1, string path2, string name)
        {
            var imageData = StereoscopyVR.ImageApp.Program.ProcessPair(path1, path2, name);
            var post = new CrossViewPost(default(Uri), name, name, 0, DateTime.Now);
            post.W = imageData.Width;
            post.H = imageData.Height;
            return post;
        }
    }
}