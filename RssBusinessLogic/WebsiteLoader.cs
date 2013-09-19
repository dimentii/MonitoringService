﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RssBusinessLogic
{
    // Loads html pages by article's links
    class WebsiteLoader
    {
        private readonly HttpClient _httpClient;

        public WebsiteLoader()
        {
            _httpClient = new HttpClient();
        }

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
                // todo: replace with log4net
                var path = @"C:\Users\Dydewki\Documents\Visual Studio 2012\Projects\Service\MonitoringService\WebsiteLoaderError.txt";
                File.AppendAllText(path, String.Format("Exception: {0}. Occured at WebsiteLoader. Link: {1}", exception.Message, link));
                return null;
            }
        }
    }
}
