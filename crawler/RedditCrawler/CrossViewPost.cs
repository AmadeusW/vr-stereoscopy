using StereoscopyVR.RedditCrawler.Endpoints;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StereoscopyVR.RedditCrawler
{
    class CrossViewPost
    {
        public Uri Url { get; }
        public string Title { get; }
        public string Link { get; }
        public int Score { get; }
        public DateTime UploadDate { get; }
        public DateTime CrawlDate { get; }

        public string ShortLink => "http://redd.it/" + Link;
        public Uri ImageUrl { get; private set; }

        public CrossViewPost(Uri url, string title, string link, int score, DateTime uploadDate)
        {
            Url = url;
            Title = title;
            Link = link.Contains("/") ? new Uri(link).PathAndQuery.Trim('/') : link;
            Score = score;
            UploadDate = uploadDate;
            CrawlDate = DateTime.UtcNow;
        }

        internal async Task TryGetImageUrl()
        {
            if (Url.Host == "i.redd.it")
            {
                ImageUrl = Url;
            }
            else if (Url.Host == "www.flickr.com")
            {
                ImageUrl = Url;
                // TODO: https://www.flickr.com/services/api/flickr.photos.getSizes.html
            }
            else if (Url.Host == "imgur.com" || Url.Host == "i.imgur.com")
            {
                if (Url.PathAndQuery.StartsWith("/a/"))
                {
                    var query = Url.PathAndQuery.Substring(3); // remove /a/
                    if (query.IndexOf('.') > -1)
                        query = query.Substring(0, query.IndexOf('.'));

                    var details = Imgur.GetDetails(query);
                    // This is an album. Generate multiple images instead
                    ImageUrl = Url;
                }
                if (Url.PathAndQuery.EndsWith(".jpg") || Url.PathAndQuery.EndsWith(".png"))
                {
                    ImageUrl = Url;
                }
                else
                {
                    ImageUrl = new Uri(Url.ToString() + ".jpg") ; // just a guess. Can we know for sure?
                }
            }
            else
            {
                ImageUrl = null;
            }
        }
    }
}
