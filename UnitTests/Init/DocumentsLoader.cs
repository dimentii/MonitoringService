﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using HtmlAgilityPack;
using WebsiteWorkers;

namespace UnitTests.Init
{
    class DocumentsLoader
    {
        public List<FeedItem> LoadDocuments(List<FeedItem> articles, IWebWorker webWorker)
        {
            var failedArticles = new List<FeedItem>();
            foreach (var article in articles)
            {
                var htmlDocument = new HtmlDocument();
                var htmlString = GetHtmlAsString(article.Link, webWorker.WebsiteEncoding);
                if (String.IsNullOrEmpty(htmlString))
                {
                    failedArticles.Add(article);
                    continue;
                }
                htmlDocument.LoadHtml(htmlString);
                article.Document = htmlDocument;
            }

            return articles.Where(art => art.Document != null).ToList();
        }

        private String GetHtmlAsString(String link, Encoding encode)
        {
            try
            {
                var responseString = String.Empty;
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(link);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var responseContent = response.Result.Content;
                        var responseBytes = responseContent.ReadAsByteArrayAsync().Result;
                        var convertedResponseBytes = Encoding.Convert(encode, Encoding.UTF8, responseBytes);
                        responseString =
                            Encoding.UTF8.GetString(convertedResponseBytes);
                    }
                }
                return responseString;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    }
}