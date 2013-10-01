using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WebsiteWorkers;

namespace RssBusinessLogic.Interfaces
{
    public interface IWebsiteParser
    {
        Task<CompleteArticleData> ParseDocuments(ArticleData data, Func<HtmlDocument, CompleteArticleData> getArticleText);
    }
}