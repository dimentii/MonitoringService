using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using RssDataAccessLayer;

namespace RssBusinessLogic.Interfaces
{
    public interface IDatabaseWorker
    {
        Task<List<String>> FillDbWithRssAsync(String table, String xmlRss, Func<DbDataReader, Task<String>> getArticleLink);
        Task<Int32> FillDbWithCompleteArticleAsync(String table, CompleteArticleData[] data);
    }
}