using System;
using System.Text;
using HtmlAgilityPack;

namespace WebsiteWorkers.Workers
{
    public class KorrespondentNet: Worker
    {
        #region Constructors

        #endregion

        #region Fields

        private readonly Encoding _websiteEncoding = Encoding.GetEncoding("windows-1251");

        #endregion

        #region Properties

        protected override RssLinkContainer Container
        {
            get
            {
                return RssLinkContainer.Link;
            }
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

        protected override HtmlNode GetIntroNode(HtmlNode articleNode)
        {
            return null;
        }

        protected override HtmlNodeCollection GetAtricleNodes(HtmlNode articleNode)
        {
            return articleNode.SelectNodes("div|p/text()");
        }

        protected override String GetAuthor(HtmlDocument document)
        {
            return String.Empty;
        }

        protected override HtmlNode GetDescriptionNode(HtmlDocument document)
        {
            return GetTitleNode(document);
        }

        protected override HtmlNode GetMainNode(HtmlDocument document)
        {
            return document.GetElementbyId("0097f");
            //this is unique too and will be helpfull if upper selector breaks;
            //return document.DocumentNode.SelectSingleNode("//div[@class='article_box']/span");
        }

        #endregion
    }
}
