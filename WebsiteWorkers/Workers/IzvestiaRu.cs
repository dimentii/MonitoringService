using System;
using System.Data.Common;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebsiteWorkers.Workers
{
    public class IzvestiaRu : Worker
    {
        public override async Task<String> GetLink(DbDataReader reader)
        {
            return await reader.GetFieldValueAsync<String>(1);
        }

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
    }
}