using System;
using System.IO;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RssBusinessLogic.Interfaces;
using RssDataAccessLayer;

namespace RssBusinessLogic
{
    class WebsiteParser : IWebsiteParser
    {
        #region Methods

        public async Task<CompleteArticleData> ParseDocuments(ArticleData data,
                                                              Func<HtmlDocument, String> getArticleText)
        {
            try
            {
                if (data == null)
                {
                    return null;
                }
                var articleString = await Task.Run(() =>
                    {
                        var htmlDocument = new HtmlDocument();

                        htmlDocument.LoadHtml(data.FullHtmlDocument);

                        return getArticleText(htmlDocument);
                    });
                return new CompleteArticleData {Article = articleString, Link = data.Link};
            }
            catch (Exception exception)
            {
                // todo: replace with log4net
                var path =
                    @"C:\Users\Dydewki\Documents\Visual Studio 2012\Projects\Service\MonitoringService\WebsiteParserError.txt";
                File.AppendAllText(path,
                                   String.Format("Exception: {0}. Occured at WebsiteParser. Link: {1}",
                                                 exception.Message, data.Link) + "\n");

                return null;
            }
        }

        #endregion
    }
}