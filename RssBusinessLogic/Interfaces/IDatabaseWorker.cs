using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteWorkers;

namespace RssBusinessLogic.Interfaces
{
    public interface IDatabaseWorker
    {
        Task<List<String>> FillDbWithRssAsync(String table, String xmlRss, IDbWorker dbWorker);
        Task<Int32> FillDbWithCompleteArticleAsync(String table, CompleteArticleData[] data);
        Task RemoveUnhandledLinks(String table, String link);
    }
}