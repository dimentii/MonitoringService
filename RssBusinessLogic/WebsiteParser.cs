using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NLog;
using RssBusinessLogic.Interfaces;
using WebsiteWorkers;

namespace RssBusinessLogic
{
    class WebsiteParser : IWebsiteParser
    {
        #region Methods

        public async Task<CompleteArticleData> ParseDocuments(ArticleData data, IWebWorker webWorker)
        {
            if (data == null)
            {
                return null;
            }
            try
            {
                var articleData = await Task.Run(() =>
                    {
                        var htmlDocument = new HtmlDocument();

                        htmlDocument.LoadHtml(data.FullHtmlDocument);

                        return webWorker.GetData(htmlDocument);
                    });

                articleData.Link = data.Link;

                return articleData;
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