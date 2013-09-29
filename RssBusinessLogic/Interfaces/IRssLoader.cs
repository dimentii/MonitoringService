using System;
using System.Threading.Tasks;

namespace RssBusinessLogic.Interfaces
{
    public interface IRssLoader
    {
        Task<String> GetRssDataByUriAsync(String feedUriString);
    }
}
