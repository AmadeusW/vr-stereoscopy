using System;
using StereoscopyVR.CoreData.Endpoints;
using System.Collections.Generic;
using StereoscopyVR.CoreData;
using System.Threading.Tasks;

namespace FlickrCrawler
{
    class Program
    {
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
            Console.WriteLine("Fetching first piece///");
            await flickr.Sample();
            var posts = await flickr.GetOriginalData("72157648972433403");
        }
    }
}