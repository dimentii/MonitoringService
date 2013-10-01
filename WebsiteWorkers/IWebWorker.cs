using HtmlAgilityPack;

namespace WebsiteWorkers
{
    public interface IWebWorker
    {
        CompleteArticleData GetData(HtmlDocument document);
    }
}
