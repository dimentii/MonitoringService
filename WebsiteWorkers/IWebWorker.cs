using System.Text;
using HtmlAgilityPack;

namespace WebsiteWorkers
{
    public interface IWebWorker
    {
        Encoding WebsiteEncoding { get; }

        CompleteArticleData GetData(HtmlDocument document);
    }
}
