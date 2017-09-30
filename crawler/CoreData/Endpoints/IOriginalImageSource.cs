using StereoscopyVR.CoreData.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StereoscopyVR.CoreData.Endpoints
{
    internal interface IOriginalImageSource
    {
        Task<IEnumerable<OriginalImage>> GetOriginalData(string id);
    }
}