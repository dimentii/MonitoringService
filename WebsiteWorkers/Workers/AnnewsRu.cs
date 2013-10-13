using System;
using System.Text;
using HtmlAgilityPack;

namespace WebsiteWorkers.Workers
{
    public class AnnewsRu : Worker
    {
        #region Fields

        private readonly Encoding _websiteEncoding = Encoding.GetEncoding("windows-1251");

        #endregion

        #region Properties

        protected override RssLinkContainer Container
        {
            get { return RssLinkContainer.Link; }
        }

        public override Encoding WebsiteEncoding
        {
            get
            {
                return _websiteEncoding;
            }
        }

        #endregion

        #region Methods

        protected override HtmlNode GetMainNode(HtmlDocument document)
        {
            return document.DocumentNode.SelectSingleNode("//div[@id='hypercontext']/index");
        }

        protected override HtmlNode GetIntroNode(HtmlNode articleNode)
        {
            return null;
        }

        protected override HtmlNodeCollection GetAtricleNodes(HtmlNode articleNode)
        {
            return articleNode.SelectNodes("h4");
        }

        protected override String GetAuthor(HtmlDocument document)
        {
            return String.Empty;
        }

        protected override HtmlNode GetDescriptionNode(HtmlDocument document)
        {
            return GetTitleNode(document);
        }

        protected override string GetDescriptionText(HtmlDocument document)
        {
            var descriptionNode = GetDescriptionNode(document);
            return descriptionNode == null ? String.Empty : descriptionNode.InnerText;
        }

        protected override HtmlNode GetTitleNode(HtmlDocument document)
        {
            return document.DocumentNode.SelectSingleNode("//title");
        }

        protected override string GetTitle(HtmlDocument document)
        {
            var titleNode = GetTitleNode(document);

            return titleNode == null ? String.Empty : titleNode.InnerText;
        }

        #endregion

    }
}
