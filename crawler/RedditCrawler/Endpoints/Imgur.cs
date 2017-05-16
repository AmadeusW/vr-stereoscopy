using System;
using System.Collections.Generic;
using System.Text;
using Refit;
using System.Threading.Tasks;

namespace StereoscopyVR.RedditCrawler.Endpoints
{
    internal static class Imgur
    {
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

        internal async static Task GetDetails(string hash)
        {
            // todo: check if this is an album
            try
            {
                var albumImages = await ImgurApi.GetAlbumImages(hash, "Client-ID " + Program.Configuration["imgur - token"]);
                Console.WriteLine("Hello World!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    public interface IImgurApi
    {
        //[Headers("Authorization: Client-ID 123")]
        //[Headers("Authorization: Client-ID " + Program.Configuration["imgur-token"])]
        [Get("/album/{hash}/images")]
        Task<List<Image>> GetAlbumImages(string hash, [Header("Authorization")] string authorization);
    }

    public class Image
    {
        public string id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string link { get; set; }
    }
}
