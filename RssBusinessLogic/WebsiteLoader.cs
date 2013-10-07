using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NLog;
using RssBusinessLogic.Interfaces;

namespace RssBusinessLogic
{
    // Loads html pages by article's links
    class WebsiteLoader : IWebsiteLoader
    {
        #region Methods

        #region Public Methods

        public async Task<ArticleData> GetWebDocumentAsync(String table, String link, Encoding websiteEncoding, Func<String, String, Task> removeUnhandledLinks)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(link, HttpCompletionOption.ResponseContentRead);

                if (response.IsSuccessStatusCode)
                {
                    var responseBytes = await response.Content.ReadAsByteArrayAsync();
                    var convertedBytes = Encoding.Convert(websiteEncoding, Encoding.UTF8, responseBytes);

                    return new ArticleData(Encoding.UTF8.GetString(convertedBytes), link);
                }
                
                await HandleFailLoad(table, link, removeUnhandledLinks);
                
                return null;
            }
        }

        #endregion

        #region Private Methods

        private static async Task HandleFailLoad(String table, String link, Func<String, String, Task> removeUnhandledLinks)
        {
            Logger logger = LogManager.GetCurrentClassLogger();

            logger.Error("Can't get response from {0}.", link);

            await removeUnhandledLinks(table, link);
        }

        #endregion

        #endregion
    }
}