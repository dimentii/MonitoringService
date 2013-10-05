using System;
using System.Text;
using HtmlAgilityPack;

namespace WebsiteWorkers.Workers
{
    public class InterfaxRu: Worker
    {
        #region Constructors

        #endregion

        #region Fields

        private const Unique ArticleIdentifier = Unique.Link;

        private readonly Encoding _websiteEncoding = Encoding.GetEncoding("windows-1251");

        #endregion

        #region Properties

        public override Unique Identifier
        {
            get { return ArticleIdentifier; }
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
            return articleNode.SelectNodes("p");
        }

        protected override String GetAuthor(HtmlDocument document)
        {
            return String.Empty;
        }
        
        protected override HtmlNode GetMainNode(HtmlDocument document)
        {
            return document.DocumentNode.SelectSingleNode("//div[@class='txtmain _ga1_on_ _reachbanner_']");
        }

        #endregion
    }
}
