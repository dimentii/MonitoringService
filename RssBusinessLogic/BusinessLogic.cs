using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RssBusinessLogic.Interfaces;
using RssDataAccessLayer;
using WebsiteWorkers;

namespace RssBusinessLogic
{
    public class BusinessLogic : IBusinessLogic
    {
        #region Fields

        private readonly IDatabaseWorker _databaseWorker;
        private readonly IWebsiteLoader _websiteLoader;
        private readonly IWebsiteParser _websiteParser;

        #endregion

        #region Constructors

        public BusinessLogic(IDatabaseWorker databaseWorker, IWebsiteLoader websiteLoader, IWebsiteParser websiteParser)
        {
            _databaseWorker = databaseWorker;
            _websiteLoader = websiteLoader;
            _websiteParser = websiteParser;
        }

        #endregion

        #region Methods

        #region Public

        // First, fill Database with data which was got as xml string. Then fill Database with
        // filtered and prepared articles Then get newspaper and number of added articles.
        public async Task<ReportData> ProcessData<TWorker>(String table, String xml, TWorker worker)
            where TWorker: IDbWorker, IWebWorker
        {
            try
            {
                var htmlDocuments = Task.WhenAll((await _databaseWorker.FillDbWithRssAsync(table, xml, worker))
                                                     .Select(
                                                         async rssDataArticle =>
                                                         await GetArticleAsync(rssDataArticle, worker.GetText)));
                return
                    new ReportData(await _databaseWorker.FillDbWithCompleteArticleAsync(table, await htmlDocuments),
                                   table);
            }
            catch (Exception exception)
            {
                // todo: replace with log4net
                var path =
                    String.Format(
                        @"C:\Users\Dydewki\Documents\Visual Studio 2012\Projects\Service\MonitoringService\BusinessLogicError{0}.txt",
                        table);
                File.AppendAllText(path, exception.Message + " Occured at: BusinessLogic" + "\n");
                return new ReportData(0, table);
            }
        }

        #endregion

        #region Private

        // Parse web documents with HTML Agility Pack after getting html page with article with WebClient.
        private async Task<CompleteArticleData> GetArticleAsync(String link, Func<HtmlDocument, String> getArticleText)
        {
            return await _websiteParser.ParseDocuments(await _websiteLoader.GetWebDocumentAsync(link), getArticleText);
        }

        #endregion

        #endregion
    }
}