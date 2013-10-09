using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssDataAccessLayer.Interfaces
{
    public interface IDataAccessLayer
    {
        Task<List<String>> FillRssAsync(String sqlCommandString);
        Task<Int32> FillCompleteDataAsync(String sqlCommandString);
        Task RemoveUnhandledArticlesAsync(String sqlCommandString);
    }
}