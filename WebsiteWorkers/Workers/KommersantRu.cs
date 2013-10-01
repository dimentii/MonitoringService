using System;
using HtmlAgilityPack;

namespace WebsiteWorkers.Workers
{
    public class KommersantRu : Worker
    {
        #region Constructor

        public KommersantRu(Identifier identifier)
            : base(identifier)
        {
        }

        #endregion

        #region Methods

        protected override HtmlNode GetIntroNode(HtmlNode articleNode)
        {
            return articleNode.SelectSingleNode("div[@class='document_vvodka']/text()");
        }

        protected override HtmlNodeCollection GetAtricleNodes(HtmlNode articleNode)
        {
            return articleNode.SelectNodes("div[@class='document_text']/text()");
        }

        protected override String GetAuthor(HtmlDocument document)
        {
            var authorNode = GetAuthorNode(document);
            if (authorNode == null)
                return String.Empty;
            return authorNode.InnerText;
        }

        protected override HtmlNode GetMainNode(HtmlDocument document)
        {
            return document.GetElementbyId("divLetterBranding");
        }

        private HtmlNode GetAuthorNode(HtmlDocument document)
        {
            return document.DocumentNode.SelectSingleNode("//div[@class='document_authors vblock']");
        }

        #endregion
    }
}
