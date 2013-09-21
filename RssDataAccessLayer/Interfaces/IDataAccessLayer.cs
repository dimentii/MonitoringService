using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace RssDataAccessLayer.Interfaces
{
    public interface IDataAccessLayer
    {
        Task<List<String>> FillRssAsync(String sqlCommandString, Func<DbDataReader, Task<String>> getArticleLink);
        Task<Int32> FillDataAsync(String sqlCommandString);
    }
}