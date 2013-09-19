using System;
using System.Data.Common;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebsiteWorkers
{
    public interface IWorker
    {
        Task<String> GetLink(DbDataReader reader);
        String GetText(HtmlDocument document);
    }
}
