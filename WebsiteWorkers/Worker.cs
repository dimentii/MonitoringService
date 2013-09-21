using System;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace WebsiteWorkers
{
    // Main class for website's parsers
    public abstract class Worker: IWorker
    {
        public abstract Task<String> GetLink(DbDataReader reader);

        public String GetText(HtmlDocument document)
        {
            var articleNode = GetMainNode(document);

            if (articleNode == null)
            {
                return GetDescriptionOnlyText(document);
            }

            var result = new StringBuilder();

            var introNode = GetIntroNode(articleNode);
            if (introNode != null)
            {
                result.Append(introNode.InnerText + " ").Replace('\'', '"');
            }

            var nodes = GetAtricleNodes(articleNode);

            if (nodes == null)
            {
                return "Only Description" + GetDescriptionOnlyText(document);
            }
            
            result.Append(GrabText(nodes));

            return result.ToString();
        }

        protected String GrabText(HtmlNodeCollection nodes)
        {
            var result = new StringBuilder();

            for (var i = 0; i < nodes.Count; i++)
            {
                result.Append(nodes[i].InnerText + " ");
            }
            result.Replace('\'', '"');
            
            return DecodeHtml(result.ToString());
        }

        protected abstract HtmlNode GetIntroNode(HtmlNode articleNode);

        protected abstract HtmlNodeCollection GetAtricleNodes(HtmlNode articleNode);

        protected abstract HtmlNode GetMainNode(HtmlDocument document);

        private String DecodeHtml(String article)
        {
            return HttpUtility.HtmlDecode(article);
        }

        private HtmlNode GetAlternateNode(HtmlDocument document)
        {
            return document.DocumentNode.SelectSingleNode("//meta[@name='description']");
        }

        private String GetDescriptionOnlyText(HtmlDocument document)
        {
            var descriptionNode = GetAlternateNode(document);
            return descriptionNode.GetAttributeValue("content", "Not found");

        }
    }
}