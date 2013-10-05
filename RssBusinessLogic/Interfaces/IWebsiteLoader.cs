using System;
using System.Text;
using System.Threading.Tasks;

namespace RssBusinessLogic.Interfaces
{
    public interface IWebsiteLoader
    {
        Task<ArticleData> GetWebDocumentAsync(String link, Encoding websiteEncoding);
    }
}