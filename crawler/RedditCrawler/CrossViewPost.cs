using StereoscopyVR.RedditCrawler.Endpoints;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Linq;

namespace StereoscopyVR.RedditCrawler
{
    public class SceneCollection
    {
        public IEnumerable<CrossViewPost> scenes { get; set; }
    }

    public class CrossViewPost
    {
        public Uri Url { get; }
        public string Title { get; }
        public string Link { get; }
        public int Score { get; }
        public DateTime UploadDate { get; }
        public DateTime CrawlDate { get; }

        public string ShortLink => "http://redd.it/" + Link;
        public Uri ImageUrl { get; set; }
        public int W { get; set; }
        public int H { get; set; }

        static Regex flickrRegex = new Regex(@"photos\/[^\/]+\/(\d+)\/.*");
        static IOriginalImageSource flickrEndpoint = new Flickr();
        static IOriginalImageSource imgurEndpoint = new Imgur();

        public CrossViewPost(Uri url, Uri imageUrl, string title, string link, int score, DateTime uploadDate)
        {
            Url = url;
            ImageUrl = imageUrl;
            Title = title;
            Link = link.Contains("/") ? new Uri(link).PathAndQuery.Trim('/') : link;
            Score = score;
            UploadDate = uploadDate;
            CrawlDate = DateTime.UtcNow;
        }

        private IEnumerable<CrossViewPost> AsEnumerable()
        {
            yield return this;
        }

        internal async Task<IEnumerable<CrossViewPost>> WithUpdatedImageUrl()
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

                return details.Select(d => new CrossViewPost(this.Url, new Uri(d.Url), String.IsNullOrEmpty(d.Title) ? this.Title : d.Title, this.Link, this.Score, this.UploadDate));
            }
            else if (Url.Host == "imgur.com" || Url.Host == "i.imgur.com")
            {
                if (Url.PathAndQuery.StartsWith("/a/") || Url.PathAndQuery.StartsWith("/gallery/"))
                {
                    var query = Url.PathAndQuery.Substring(Url.PathAndQuery.Trim('/').IndexOf('/') + 2); // remove /a/ or /gallery/
                    if (query.IndexOf('.') > -1)
                        query = query.Substring(0, query.IndexOf('.'));

                    var details = await imgurEndpoint.GetOriginalData(query);
                    return details.Select(d => new CrossViewPost(this.Url, new Uri(d.Url), String.IsNullOrEmpty(d.Title) ? this.Title : d.Title, this.Link, this.Score, this.UploadDate));
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
            return new CrossViewPost[0].AsEnumerable();
        }
    }
}
