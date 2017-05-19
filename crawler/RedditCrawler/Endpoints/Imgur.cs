using System;
using System.Collections.Generic;
using System.Text;
using Refit;
using System.Threading.Tasks;
using StereoscopyVR.RedditCrawler.Data;

namespace StereoscopyVR.RedditCrawler.Endpoints
{
    internal class Imgur : IOriginalImageSource
    {
        public async Task<OriginalImage> GetOriginalData(string id)
        {
            // TODO: Check if this is an album
            var albumImages = await ImgurApi.GetAlbumImages(id, "Client-ID " + Program.Configuration["imgur-token"]);
            return new OriginalImage
            {
                Url = albumImages.data[0].link.ToString()
            };
        }

        static IImgurApi _imgurApi;
        static IImgurApi ImgurApi
        {
            get
            {
                if (_imgurApi == null)
                {
                    var settings = new RefitSettings()
                    {
                        //AuthorizationHeaderValueGetter = () => Task.FromResult("Client-ID 123"),
                        AuthorizationHeaderValueGetter = () => Task.FromResult("Client-ID " + Program.Configuration["imgur-token"]),
                    };
                    _imgurApi = RestService.For<IImgurApi>("https://api.imgur.com/3/");//, settings);
                }
                return _imgurApi;
            }
        }
    }

    public interface IImgurApi
    {
        //[Headers("Authorization: Client-ID 123")]
        //[Headers("Authorization: Client-ID " + Program.Configuration["imgur-token"])]
        [Get("/album/{hash}/images")]
        Task<ImgurData> GetAlbumImages(string hash, [Header("Authorization")] string authorization);
    }

    public class ImgurData
    {
        public List<ImgurImage> data { get; set; }
    }

    public class ImgurImage
    {
        public string id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string link { get; set; }
    }
}
