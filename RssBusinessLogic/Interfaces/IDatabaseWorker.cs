using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RssBusinessLogic.RSS;
using WebsiteWorkers;

namespace RssBusinessLogic.Interfaces
{
    public interface IDatabaseWorker
    {
        Task<List<String>> FillDbWithRssAsync(String table, Channel channel, IDbWorker dbWorker);
        Task<Int32> FillDbWithCompleteArticleAsync(String table, CompleteArticleData[] data);
        Task RemoveUnhandledLinks(String table, String link);
    }
}