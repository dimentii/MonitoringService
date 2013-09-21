using System;
using System.Threading.Tasks;

namespace RssBusinessLogic.Interfaces
{
    public interface IWebsiteLoader
    {
        Task<ArticleData> GetWebDocumentAsync(String link);
    }
}