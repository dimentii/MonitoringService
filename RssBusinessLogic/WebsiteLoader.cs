using System;
using System.Net.Http;
using System.Threading.Tasks;
using NLog;
using RssBusinessLogic.Interfaces;

namespace RssBusinessLogic
{
    // Loads html pages by article's links
    class WebsiteLoader : IWebsiteLoader
    {
        #region Fields

        private readonly HttpClient _httpClient;

        #endregion

        #region Constructors

        public WebsiteLoader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion

        #region Methods

        public async Task<ArticleData> GetWebDocumentAsync(String link)
        {
            try
            {
                var response = await _httpClient.GetAsync(link, HttpCompletionOption.ResponseContentRead);
                response.EnsureSuccessStatusCode();
                return new ArticleData(await response.Content.ReadAsStringAsync(), link);
            }
            catch (Exception exception)
            {
                Logger logger = LogManager.GetCurrentClassLogger();

                logger.Error("Can't get response from {0}. Error message: {1}", link, exception.Message);

                return null;
            }
        }

        #endregion
    }
}
