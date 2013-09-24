using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using ConsoleApplication1.Interfaces;
using RssBusinessLogic;
using RssBusinessLogic.Interfaces;
using WebsiteWorkers;
using WebsiteWorkers.Workers;

namespace ConsoleApplication1
{
    class RssHandler : IRssHandler
    {
        #region Fields

        // Field for list of websites
        private readonly List<WebsiteInput> _websiteInputs = new List<WebsiteInput>
            {
                new WebsiteInput("Kommersant", "http://kommersant.ru/rss/daily.xml", new KommersantRu(Identifier.Guid)),
                new WebsiteInput("Izvestia", "http://izvestia.ru/xml/rss/all.xml", new IzvestiaRu(Identifier.Link)),
                new WebsiteInput("Korrespondent", "http://k.img.com.ua/rss/ru/news.xml", new KorrespondentNet(Identifier.Link))
            };

        private readonly IBusinessLogic _businessLogic;

        #endregion

        #region Constructors

        public RssHandler(IBusinessLogic businessLogic)
        {
            _businessLogic = businessLogic;
        }

        #endregion

        #region Methods

        #region Public

        // Main method
        public async Task<List<ReportData>> HandleRssAsync()
        {
            var newRecords = await Task.WhenAll(_websiteInputs.Select(
                async websiteInput => _businessLogic.ProcessData(websiteInput.Newspaper,
                    await GetRssDataByUriAsync(websiteInput.XmlLink), websiteInput.Worker)));

            var records = new List<ReportData>();
            foreach (var record in newRecords)
            {
                records.Add(await record);
            }
            return records;
        }

        #endregion

        #region Private

        // Get Data from websites in rss format
        private static async Task<String> GetRssDataByUriAsync(String feedUriString)
        {
            try
            {
                var rssString = String.Empty;
                using (var xmlReader = XmlReader.Create(feedUriString, 
                    new XmlReaderSettings { Async = true, IgnoreProcessingInstructions = true }))
                {
                    while (await xmlReader.ReadAsync())
                    {
                        rssString = xmlReader.ReadInnerXml();
                    }
                }
                return rssString;
            }
            catch (Exception exception)
            {
                // todo: replace with log4net
                var path =
                    @"C:\Users\Dydewki\Documents\Visual Studio 2012\Projects\Service\MonitoringService\RssHandler.txt";
                File.AppendAllText(path, exception.Message + " Occured at: RssHandler. Incorrect link: " + feedUriString);
                return null;
            }
        }

        #endregion

        #endregion
    }
}