using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using RssBusinessLogic.Interfaces;
using WebsiteWorkers;

namespace RssBusinessLogic
{
    public class BusinessLogic : IBusinessLogic
    {
        #region Fields

        private readonly IDatabaseWorker _databaseWorker;
        private readonly IWebsiteLoader _websiteLoader;
        private readonly IWebsiteParser _websiteParser;
        private readonly IRssLoader _rssLoader;

        readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors

        public BusinessLogic(IDatabaseWorker databaseWorker, IWebsiteLoader websiteLoader, IWebsiteParser websiteParser, IRssLoader rssLoader)
        {
            _databaseWorker = databaseWorker;
            _websiteLoader = websiteLoader;
            _websiteParser = websiteParser;
            _rssLoader = rssLoader;
        }

        #endregion

        #region Methods

        #region Public

        public async Task BeginWork(List<WebsiteInput> inputs)
        {
            var reports = await Task.WhenAll(inputs.Select(async websiteInput => ProcessData(websiteInput.Newspaper,
                await _rssLoader.GetRssDataByUriAsync(websiteInput.XmlLink), websiteInput.Worker)));

            foreach (var report in reports)
            {
                _logger.Info((await report).ToString());
            }
        }

        #endregion

        #region Private

        /* First, fill Database with data which was got as xml string. Then fill Database with
         filtered and prepared articles Then get newspaper and number of added articles. */
        private async Task<ReportData> ProcessData<TWorker>(String table, String xml, TWorker worker)
            where TWorker : IDbWorker, IWebWorker
        {
            try
            {
                var htmlDocuments = Task.WhenAll((await _databaseWorker.FillDbWithRssAsync(table, xml, worker))
                                                     .Select(async rssDataArticle => await GetArticleAsync(rssDataArticle, worker)));
                return
                    new ReportData(await _databaseWorker.FillDbWithCompleteArticleAsync(table, await htmlDocuments), table);
            }
            catch (Exception exception)
            {
                _logger.Error("Error while processing data for {0} at business logic. Error message: {1}", table, exception.Message);

                return new ReportData(0, table);
            }
        }

        // Parse web documents with HTML Agility Pack after getting html page with article with WebClient.
        private async Task<CompleteArticleData> GetArticleAsync(String link, IWebWorker webWorker)
        {
            return await _websiteParser.ParseDocuments(await _websiteLoader.GetWebDocumentAsync(link, webWorker.WebsiteEncoding), webWorker);
        }

        #endregion

        #endregion
    }
}