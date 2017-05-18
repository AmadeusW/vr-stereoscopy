using System;
using System.Collections.Generic;
using System.Text;
using Refit;
using System.Threading.Tasks;

namespace StereoscopyVR.RedditCrawler.Endpoints
{
    internal static class Flickr
    {
        static IFlickrApi _flickrApi;
        static IFlickrApi FlickrApi
        {
            get
            {
                if (_flickrApi == null)
                {
                    var settings = new RefitSettings()
                    {
                        //AuthorizationHeaderValueGetter = () => Task.FromResult("Client-ID 123"),
                        AuthorizationHeaderValueGetter = () => Task.FromResult("Client-ID " + Program.Configuration["flickr-token"]),
                    };
                    _flickrApi = RestService.For<IFlickrApi>("https://api.flickr.com/services");//, settings);
                }
                return _flickrApi;
            }
        }

        internal async static Task GetDetails(string photoId)
        {
            // todo: check if this is an album
            try
            {
                var albumImages = await FlickrApi.GetAlbumImages(photoId, Program.Configuration["flickr-token"]);
                Console.WriteLine("Flickr: " + albumImages.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    public interface IFlickrApi
    {
        [Get("/rest?method=flickr.photos.getSizes&api_key={api_key}&photo_id={photo_id}&format=json")]
        Task<string> GetAlbumImages(string photo_id, string api_key);
    }

    public class ImageSizes
    {
        public AllSizes sizes { get; set; }
    }
    
    public class AllSizes
    {
        public RawImageData[] size { get; set; }
        public int canblog { get; set; }
        public int candownload { get; set; }
        public int canprint { get; set; }
    }

    public class RawImageData
    {
        public string label { get; set; }
        public string source { get; set; }
        public string url { get; set; }
        public string media { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
