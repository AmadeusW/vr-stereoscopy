using StereoscopyVR.RedditCrawler.Data;
using System.Threading.Tasks;

namespace StereoscopyVR.RedditCrawler.Endpoints
{
    internal interface IOriginalImageSource
    {
        Task<OriginalImage> GetOriginalData(string id);
    }
}