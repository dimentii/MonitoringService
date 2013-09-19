using System;

namespace RssBusinessLogic
{
    public class ArticleData
    {
        public ArticleData(String fullHtmlDocument, String link)
        {
            FullHtmlDocument = fullHtmlDocument;
            Link = link;
        }
        public String FullHtmlDocument { get; set; }
        public String Link { get; set; }
    }
}
