using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RssDataAccessLayer;

namespace RssBusinessLogic.Interfaces
{
    public interface IWebsiteParser
    {
        Task<CompleteArticleData> ParseDocuments(ArticleData data, Func<HtmlDocument, String> getArticleText);
    }
}