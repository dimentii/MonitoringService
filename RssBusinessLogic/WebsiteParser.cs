using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NLog;
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
            if (data == null)
            {
                return null;
            }
            try
            {
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
                Logger logger = LogManager.GetCurrentClassLogger();

                logger.Error("Can't parse Html Document {0}. Error message: {1}", data.Link, exception.Message);

                return null;
            }
        }

        #endregion
    }
}