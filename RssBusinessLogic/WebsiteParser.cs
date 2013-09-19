using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RssDataAccessLayer;
using WebsiteWorkers;

namespace RssBusinessLogic
{
    public class WebsiteParser
    {
        public async Task<CompleteArticleData> ParseDocuments(ArticleData data, GetArticleText getArticleText)
        {
            try
            {
                if (data == null)
                {
                    return null;
                }
                var articleString = await Task.Run(() =>
                    {
                        var htmlDocument = new HtmlDocument();

                        htmlDocument.LoadHtml(data.FullHtmlDocument);
                        
                        return getArticleText(htmlDocument);
                    });
                return new CompleteArticleData { Article = articleString, Link = data.Link };
            }
            catch (Exception exception)
            {
                // todo: replace with log4net
                var path = @"C:\Users\Dydewki\Documents\Visual Studio 2012\Projects\Service\MonitoringService\WebsiteParserError.txt";
                File.AppendAllText(path, String.Format("Exception: {0}. Occured at WebsiteParser. Link: {1}", exception.Message, data.Link));

                return null;
            }
        }

        #region Just for example

        public static HtmlNode GetNodeByTemplate(HtmlDocument document, String template)
        {
            var debugger = document.DocumentNode.SelectSingleNode("//*[normalize-space(.)='" + template + "']") ??
                           document.DocumentNode.SelectSingleNode("//*[.='" + template + "']") ??
                           document.DocumentNode.SelectSingleNode("//*[starts-with(., '" + template + "')]") ??

                           document.DocumentNode.SelectSingleNode("//*[normalize-space(.)='" +
                                                                  template.Replace("\"", "&quot;") + "']") ??
                           document.DocumentNode.SelectSingleNode("//*[.='" + template.Replace("\"", "&quot;") + "']") ??
                           document.DocumentNode.SelectSingleNode("//*[starts-with(., '" +
                                                                  template.Replace("\"", "&quot;") + "')]") ??

                           document.DocumentNode.SelectSingleNode("//*[normalize-space(.)='" +
                                                                  template.Replace("—", "&mdash;") + "']") ??
                           document.DocumentNode.SelectSingleNode("//*[.='" + template.Replace("—", "&mdash;") + "']") ??
                           document.DocumentNode.SelectSingleNode("//*[starts-with(., '" +
                                                                  template.Replace("—", "&mdash;") + "')]");
            if (debugger == null)
            {
                Debugger.Break();
            }
            return document.DocumentNode.SelectSingleNode("//*[normalize-space(.)='" + template + "']") ??
                   document.DocumentNode.SelectSingleNode("//*[.='" + template + "']") ??
                   document.DocumentNode.SelectSingleNode("//*[starts-with(., '" + template + "')]") ??

                   document.DocumentNode.SelectSingleNode("//*[normalize-space(.)='" + template.Replace("\"", "&quot;") +
                                                          "']") ??
                   document.DocumentNode.SelectSingleNode("//*[.='" + template.Replace("\"", "&quot;") + "']") ??
                   document.DocumentNode.SelectSingleNode("//*[starts-with(., '" + template.Replace("\"", "&quot;") +
                                                          "')]") ??

                   document.DocumentNode.SelectSingleNode("//*[normalize-space(.)='" + template.Replace("—", "&mdash;") +
                                                          "']") ??
                   document.DocumentNode.SelectSingleNode("//*[.='" + template.Replace("—", "&mdash;") + "']") ??
                   document.DocumentNode.SelectSingleNode("//*[starts-with(., '" + template.Replace("—", "&mdash;") +
                                                          "')]");
        }

        #endregion
    }
}