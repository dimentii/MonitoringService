using HtmlAgilityPack;

namespace WebsiteWorkers.Workers
{
    public class KorrespondentNet: Worker
    {
        #region Constructors

        public KorrespondentNet(Identifier identifier)
            : base(identifier)
        {
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

        protected override HtmlNode GetMainNode(HtmlDocument document)
        {
            return document.GetElementbyId("0097f");
            //this is unique too and will be helpfull if upper selector breaks;
            //return document.DocumentNode.SelectSingleNode("//div[@class='article_box']/span");
        }

        #endregion
    }
}
