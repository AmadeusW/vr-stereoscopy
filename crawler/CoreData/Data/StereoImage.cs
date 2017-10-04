using System;
using System.Collections.Generic;
using System.Text;

namespace StereoscopyVR.CoreData.Data
{
    public class StereoImage
    {
        /// <summary>
        /// Url to the photo page on a photo service (with comments, author info etc)
        /// </summary>
        public Uri Url { get; }
        public string Title { get; }
        /// <summary>
        /// Filename
        /// </summary>
        public string Link { get; }
        public DateTime UploadDate { get; }
        public DateTime CrawlDate { get; }
        /// <summary>
        /// Url to original raw image
        /// </summary>
        public Uri ImageUrl { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public int[] Corrections = new int[3];

        public StereoImage(Uri url, Uri imageUrl, string title, string link, DateTime uploadDate)
        {
            Url = url;
            ImageUrl = imageUrl;
            Title = title;
            Link = link.Contains("/") ? new Uri(link).PathAndQuery.Trim('/') : link;
            UploadDate = uploadDate;
            CrawlDate = DateTime.UtcNow;
        }
    }
}
