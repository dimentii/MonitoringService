using System;
using System.Collections.Generic;
using System.Net.Http;
using HtmlAgilityPack;

namespace UnitTests.Init
{
    class DocumentsLoader
    {
        public void LoadDocuments(List<FeedItem> articles)
        {
            foreach (var article in articles)
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(GetHttpAsString(article.Link));

                article.Document = htmlDocument;
            }
        }

        private String GetHttpAsString(String link)
        {
            String responseString = String.Empty;
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(link);
                if (response.Result.IsSuccessStatusCode)
                {
                    var responseContent = response.Result.Content;

                    responseString = responseContent.ReadAsStringAsync().Result;
                }
            }
            return responseString;
        }
    }
}
