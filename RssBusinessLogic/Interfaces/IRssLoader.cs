using System;
using System.Threading.Tasks;
using RssBusinessLogic.RSS;

namespace RssBusinessLogic.Interfaces
{
    public interface IRssLoader
    {
        Task<Channel> GetRssDataByUriAsync(String feedUriString);
    }
}
