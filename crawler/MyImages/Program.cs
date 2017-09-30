using StereoscopyVR.ImageApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using StereoscopyVR.CoreData.Data;

namespace MyImages
{
    class Program
    {
        const string SaveLocation = "out";
        const string PostsFile = "posts.json";

        static void Main(string[] args)
        {
            Directory.CreateDirectory(SaveLocation);
            DoWork();            
        }

        private static void DoWork()
        {
            var processedPosts = new List<StereoImage>();
            var files = GetPairs(@"C:\Users\amadeusz\Pictures\Stereo\out\").ToArray();
            for (int i = 0; i < files.Length; i += 2)
            {
                var file1 = files[i];   // Right eye view
                var file2 = files[i+1]; // Left eye view
                var name = Path.GetFileNameWithoutExtension(file1);
                if (name.EndsWith(".L") || name.EndsWith(".R"))
                {
                    name = name.Substring(0, name.Length - 2);
                }
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

        private static StereoImage ProcessPair(string path1, string path2, string name)
        {
            var imageData = StereoscopyVR.ImageApp.Program.ProcessPair(path1, path2, name);
            var post = new StereoImage(default(Uri), default(Uri), name, name, DateTime.Now);
            post.W = imageData.Width;
            post.H = imageData.Height;
            return post;
        }
    }
}