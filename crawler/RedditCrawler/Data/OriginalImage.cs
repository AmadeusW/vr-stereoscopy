using System;
using System.Collections.Generic;
using System.Text;

namespace StereoscopyVR.RedditCrawler.Data
{
    public class OriginalImage
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Author { get; set; }

        public string FileName => Url.Substring(0, Url.LastIndexOf('.')).Substring(Url.LastIndexOf('/') + 1);
    }
}
