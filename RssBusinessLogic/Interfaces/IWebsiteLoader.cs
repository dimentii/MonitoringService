using System;
using System.Text;
using System.Threading.Tasks;

namespace RssBusinessLogic.Interfaces
{
    public interface IWebsiteLoader
    {
        Task<ArticleData> GetWebDocumentAsync(String table, String link, Encoding websiteEncoding, Func<String, String, Task> removeUnhandledLinks);
    }
}