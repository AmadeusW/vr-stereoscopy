using StereoscopyVR.CoreData.Endpoints;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;

namespace StereoscopyVR.CoreData.Data
{
    public class RedditCrossViewPost : StereoImage
    {
        public string ShortLink => "http://redd.it/" + Link;
        public int Score { get; }

        static Regex flickrRegex = new Regex(@"photos\/[^\/]+\/(\d+)\/.*");
        static IOriginalImageSource flickrEndpoint = new Flickr();
        static IOriginalImageSource imgurEndpoint = new Imgur();

        public RedditCrossViewPost(Uri url, Uri imageUrl, string title, string link, int score, DateTime uploadDate)
            : base(url, imageUrl, title, link, uploadDate)
        {
            Score = score;
        }

        private IEnumerable<RedditCrossViewPost> AsEnumerable()
        {
            yield return this;
        }

        public async Task<IEnumerable<RedditCrossViewPost>> WithUpdatedImageUrl()
        {
            ImageUrl = null;

            if (Url.Host == "i.redd.it")
            {
                ImageUrl = Url;
                return this.AsEnumerable();
            }
            else if (Url.Host == "www.flickr.com")
            {
                ImageUrl = Url;
                var query = Url.PathAndQuery.TrimEnd('/');
                var photoId = flickrRegex.Match(query).Groups[1].Value;
                var details = await flickrEndpoint.GetOriginalData(photoId);

                return details.Select(d => new RedditCrossViewPost(this.Url, new Uri(d.Url), String.IsNullOrEmpty(d.Title) ? this.Title : d.Title, this.Link, this.Score, this.UploadDate));
            }
            else if (Url.Host == "imgur.com" || Url.Host == "i.imgur.com")
            {
                if (Url.PathAndQuery.StartsWith("/a/") || Url.PathAndQuery.StartsWith("/gallery/"))
                {
                    var query = Url.PathAndQuery.Substring(Url.PathAndQuery.Trim('/').IndexOf('/') + 2); // remove /a/ or /gallery/
                    if (query.IndexOf('.') > -1)
                        query = query.Substring(0, query.IndexOf('.'));

                    var details = await imgurEndpoint.GetOriginalData(query);
                    return details.Select(d => new RedditCrossViewPost(this.Url, new Uri(d.Url), String.IsNullOrEmpty(d.Title) ? this.Title : d.Title, d.FileName, this.Score, this.UploadDate));
                }
                else
                {
                    if (Url.PathAndQuery.EndsWith(".jpg") || Url.PathAndQuery.EndsWith(".png"))
                    {
                        ImageUrl = Url;
                    }
                    else
                    {
                        ImageUrl = new Uri(Url.ToString() + ".jpg"); // just a guess. Can we know for sure?
                    }
                    return this.AsEnumerable();
                }
            }
            return new RedditCrossViewPost[0].AsEnumerable();
        }
    }
}
