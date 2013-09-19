using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RssDataAccessLayer;
using WebsiteWorkers;

namespace RssBusinessLogic
{
    public class BusinessLogic
    {
        // Need to create Interfaces and use IoC (Ninject)
        private readonly DatabaseWorker _databaseWorker = new DatabaseWorker();
        private readonly WebsiteLoader _websiteLoader = new WebsiteLoader();
        private readonly WebsiteParser _websiteParser = new WebsiteParser();

        // First, fill Database with data which was got as xml string. Then fill Database with
        // filtered and prepared articles Then get newspaper and number of added articles.
        public async Task<ReportData> ProcessData(String table, String xml, IWorker worker)
        {
            try
            {
                var htmlDocuments = Task.WhenAll((await _databaseWorker.FillDbWithRssAsync(table, xml, worker.GetLink))
                    .Select(async rssDataArticle => await GetArticleAsync(rssDataArticle, worker.GetText)));
                return 
                    new ReportData(await _databaseWorker.FillDbWithCompleteArticleAsync(table, await htmlDocuments), table);
            }
            catch (Exception exception)
            {
                // todo: replace with log4net
                var path =
                    String.Format(
                        @"C:\Users\Dydewki\Documents\Visual Studio 2012\Projects\Service\MonitoringService\BusinessLogicError{0}.txt",
                        table);
                File.AppendAllText(path, exception.Message + " Occured at: BusinessLogic");
                return new ReportData(0, table);
            }
        }

        // Parse web documents with HTML Agility Pack after getting html page with article with WebClient.
        private async Task<CompleteArticleData> GetArticleAsync(String link, GetArticleText getArticleText)
        {
            return await _websiteParser.ParseDocuments(await _websiteLoader.GetWebDocumentAsync(link), getArticleText);
        }
    }
}