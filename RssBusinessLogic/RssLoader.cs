using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using NLog;
using RssBusinessLogic.Interfaces;
using RssBusinessLogic.RSS;

namespace RssBusinessLogic
{
    class RssLoader: IRssLoader
    {
        #region Methods

        #region Public

        // Get Data from websites in rss format
        public async Task<Channel> GetRssDataByUriAsync(String feedUriString)
        {
            try
            {
                Channel rssChannel = null;
                using (var xmlReader = XmlReader.Create(feedUriString,
                    new XmlReaderSettings { Async = true, IgnoreProcessingInstructions = true }))
                {
                    while (await xmlReader.ReadAsync())
                    {
                        var xmlSerializer = new XmlSerializer(typeof (Channel));
                        rssChannel = (Channel) xmlSerializer.Deserialize(xmlReader);
                    }
                }
                return rssChannel;
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
