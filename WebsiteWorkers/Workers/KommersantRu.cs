using System;
using System.Data.Common;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebsiteWorkers.Workers
{
    public class KommersantRu: Worker
    {
        public override async Task<String> GetLink(DbDataReader reader)
        {
            return await reader.GetFieldValueAsync<String>(0);
        }

        protected override HtmlNode GetIntroNode(HtmlNode articleNode)
        {
            return articleNode.SelectSingleNode("div[@class='document_vvodka']/text()");
        }

        protected override HtmlNodeCollection GetAtricleNodes(HtmlNode articleNode)
        {
            return articleNode.SelectNodes("div[@class='document_text']/text()");
        }

        protected override HtmlNode GetMainNode(HtmlDocument document)
        {
            return document.GetElementbyId("divLetterBranding");
        }
    }
}
