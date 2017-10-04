using System;
using System.Collections.Generic;
using System.Text;
using Refit;
using System.Threading.Tasks;
using StereoscopyVR.CoreData.Data;
using System.Linq;

namespace StereoscopyVR.CoreData.Endpoints
{
    public class Flickr : IOriginalImageSource
    {
        const string PhotoPageUrlFormat = @"https://www.flickr.com/photos/{0}/{1}";
        const string AlbumUrlFormat = @"https://www.flickr.com/photos/{0}/albums/{1}";
        public async Task<IEnumerable<StereoImage>> ScrapeAlbum(string userId, string albumId, string userName)
        {
            var photos = await FlickrApi.GetAlbum(userId, albumId, Configuration.Instance["flickr-token"]);
            var result = new List<StereoImage>();
            foreach (var photo in photos.photoset.photo)
            {
                Console.WriteLine($"Processing photo {photo.id}");
                var image = await GetOriginalData(photo.id);
                result.AddRange(image.Select(n => new StereoImage(
                    url: new Uri(string.Format(PhotoPageUrlFormat, userName, photo.id)),
                    imageUrl: new Uri(photo.url_o),
                    title: photo.title,
                    link: n.FileName,
                    uploadDate: default(DateTime)
                )));
            }
            return result;
        }

        public async Task<IEnumerable<OriginalImage>> GetOriginalData(string id)
        {
            try
            {
                var albumImages = await FlickrApi.GetImageUrls(id, Configuration.Instance["flickr-token"]);
                var match = albumImages.sizes.size.FirstOrDefault(n => n.label == "Original");
                if (match == null)
                {
                    match = albumImages.sizes.size.FirstOrDefault(n => n.label == "Large");
                    if (match == null)
                    {
                        return null;
                    }
                }
                return new OriginalImage[]
                {
                    new OriginalImage { Url = match.source }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task Sample()
        {
            throw new NotImplementedException();
            //var id1 = await FlickrApi.GetUserIdByName("amadeusw", Configuration.Instance["flickr-token"]);
            //var id2 = await FlickrApi.GetUserIdByUrl(@"https://www.flickr.com/photos/fotoopa_hs", Configuration.Instance["flickr-token"]);
            //var photosets = await FlickrApi.GetAlbumsByUser(id2.user.id, Configuration.Instance["flickr-token"]);
            //var photos = await FlickrApi.GetAlbum(id2.user.id, "72157648972433403", Configuration.Instance["flickr-token"]);
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
        Task<FlickrGetSizesResponse> GetImageUrls(string photo_id, string api_key);

        [Get("/rest?method=flickr.urls.lookupUser&api_key={api_key}&url={url}&format=json&nojsoncallback=1")]
        Task<FlickrUserResponse> GetUserIdByUrl(string url, string api_key);

        [Get("/rest?method=flickr.photosets.getPhotos&api_key={api_key}&photoset_id={albumId}&user_id={userId}&extras=date_uploaded,media,path_alias,url_o,license&format=json&nojsoncallback=1")]
        Task<FlickrGetPhotosResponse> GetAlbum(string userId, string albumId, string api_key);

        // helper methods not actively used

        [Get("/rest?method=flickr.people.findByUsername&api_key={api_key}&username={username}&format=json&nojsoncallback=1")]
        Task<FlickrUserResponse> GetUserIdByName(string username, string api_key);

        [Get("/rest?method=flickr.photosets.getList&api_key={api_key}&user_id={userId}&format=json&nojsoncallback=1")]
        Task<string> GetAlbumsByUser(string userId, string api_key);
    }

    public class FlickrUserResponse
    {
        public FlickrUser user {get;set;}
    }
    public class FlickrUser
    { 
        public string id{ get; set; }
        public FlickrWhatever username { get; set; }
    }

    public class FlickrWhatever
    {
        public string _content { get; set; }
    }

    public class FlickrGetPhotosResponse
    {
        public FlickrPhotoset photoset { get; set; }
    }

    public class FlickrPhotoset
    {
        public string id{ get; set; }
        public string owner { get; set; }
        public string ownername { get; set; }
        public FlickrPhoto[] photo{ get; set; }
    }

    public class FlickrPhoto
    {
        public string id { get; set; }
        public string secret { get; set; }
        public string title { get; set; }
        public string date_uploaded { get; set; }
        public string media { get; set; }
        public string path_alias { get; set; }
        public string url_o { get; set; }
        public string license { get; set; }
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
