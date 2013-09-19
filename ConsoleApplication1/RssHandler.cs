using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using RssBusinessLogic;
using WebsiteWorkers.Workers;

namespace ConsoleApplication1
{
    class RssHandler
    {
        private readonly List<WebsiteInput> _websiteInputs = new List<WebsiteInput>
            {
                new WebsiteInput("Kommersant", "http://kommersant.ru/rss/daily.xml", new KommersantRu()),
                new WebsiteInput("Izvestia", "http://izvestia.ru/xml/rss/all.xml", new IzvestiaRu()),
                new WebsiteInput("Korrespondent", "http://k.img.com.ua/rss/ru/news.xml", new KorrespondentNet())
            };

        public async Task<List<ReportData>> HandleRssAsync()
        {
            var newRecords = await Task.WhenAll(_websiteInputs.Select(async websiteInput => 
                          new BusinessLogic().ProcessData(websiteInput.Newspaper, await GetRssDataByUriAsync(websiteInput.XmlLink), websiteInput.Worker)));
            var records = new List<ReportData>();
            foreach (var record in newRecords)
            {
                records.Add(await record);
                Console.WriteLine(await record);
            }
            return records;
        }

        private static async Task<String> GetRssDataByUriAsync(String feedUriString)
        {
            try
            {
                var rssString = String.Empty;
                using (var xmlReader = XmlReader.Create(feedUriString, new XmlReaderSettings { Async = true, IgnoreProcessingInstructions = true }))
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
                Console.WriteLine(exception.Message + " Occured at: RssHandler");
                return null;
            }
        }
    }
}