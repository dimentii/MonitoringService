using HtmlAgilityPack;

namespace WebsiteWorkers.Workers
{
    public class IzvestiaRu : Worker
    {
        #region Constructor

        public IzvestiaRu(Identifier identifier)
            : base(identifier)
        {
        }

        #endregion

        #region Methods

        protected override HtmlNode GetIntroNode(HtmlNode articleNode)
        {
            return articleNode.SelectSingleNode("h2[@class='subtitle']/text()");
        }

        protected override HtmlNodeCollection GetAtricleNodes(HtmlNode articleNode)
        {
            return articleNode.SelectNodes("p");
        }

        protected override HtmlNode GetMainNode(HtmlDocument document)
        {
            return document.DocumentNode.SelectSingleNode("//div[@class='text_block']");
        }

        #endregion
    }
}