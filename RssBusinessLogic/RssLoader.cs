using System;
using System.Threading.Tasks;
using System.Xml;
using NLog;
using RssBusinessLogic.Interfaces;

namespace RssBusinessLogic
{
    class RssLoader: IRssLoader
    {
        #region Methods

        #region Public

        // Get Data from websites in rss format
        public async Task<String> GetRssDataByUriAsync(String feedUriString)
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
                Logger logger = LogManager.GetCurrentClassLogger();

                logger.Error("Error while getting RSS data from website: {0}. Error message: {1}",
                    feedUriString, exception.Message);

                return null;
            }
        }

        #endregion

        #endregion
    }
}
