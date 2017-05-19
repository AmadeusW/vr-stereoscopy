using System;
using System.Collections.Generic;
using System.Text;
using Refit;
using System.Threading.Tasks;
using StereoscopyVR.RedditCrawler.Data;
using System.Linq;

namespace StereoscopyVR.RedditCrawler.Endpoints
{
    internal class Flickr : IOriginalImageSource
    {
        public async Task<OriginalImage> GetOriginalData(string id)
        {
            try
            {
                var albumImages = await FlickrApi.GetAlbumImages(id, Program.Configuration["flickr-token"]);
                var match = albumImages.sizes.size.FirstOrDefault(n => n.label == "Original");
                if (match == null)
                {
                    match = albumImages.sizes.size.FirstOrDefault(n => n.label == "Large");
                    if (match == null)
                    {
                        return null;
                    }
                }
                return new OriginalImage
                {
                    Url = match.source
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        static IFlickrApi _flickrApi;
        static IFlickrApi FlickrApi
        {
            get
            {
                if (_flickrApi == null)
                {
                    _flickrApi = RestService.For<IFlickrApi>("https://api.flickr.com/services");
                }
                return _flickrApi;
            }
        }
    }

    public interface IFlickrApi
    {
        [Get("/rest?method=flickr.photos.getSizes&api_key={api_key}&photo_id={photo_id}&format=json&nojsoncallback=1")]
        Task<FlickrGetSizesResponse> GetAlbumImages(string photo_id, string api_key);
    }

    public class FlickrGetSizesResponse
    {
        public FlickrAllSizes sizes { get; set; }
    }
    
    public class FlickrAllSizes
    {
        public FlickrRawImage[] size { get; set; }
    }

    public class FlickrRawImage
    {
        public string label { get; set; }
        public string source { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
