using System.Threading.Tasks;
using WebsiteWorkers;

namespace RssBusinessLogic.Interfaces
{
    public interface IWebsiteParser
    {
        Task<CompleteArticleData> ParseDocuments(ArticleData data, IWebWorker webWorker);
    }
}