using StereoscopyVR.RedditCrawler.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StereoscopyVR.RedditCrawler.Endpoints
{
    internal interface IOriginalImageSource
    {
        Task<IEnumerable<OriginalImage>> GetOriginalData(string id);
    }
}