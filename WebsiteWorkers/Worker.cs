using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Web;
using HtmlAgilityPack;

namespace WebsiteWorkers
{
    // Main class for website's parsers
    public abstract class Worker: IWebWorker, IDbWorker
    {
        #region Properties

        public Identifier Identifier { get; set; }

        #endregion

        #region Constructors

        protected Worker(Identifier identifier)
        {
            Identifier = identifier;
        }

        #endregion

        #region Methods

        #region DbWorker Methods

        public String GetIdentifyingQuery()
        {
            FieldInfo fi = Identifier.GetType().GetField(Identifier.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : Identifier.ToString();
        }

        #endregion

        #region WebWorker Methods

        public String GetText(HtmlDocument document)
        {
            var result = new StringBuilder();

            #region Article Main Node

            var articleNode = GetMainNode(document);

            if (articleNode == null)
            {
                return GetDescriptionOnlyText(document);
            }

            #endregion

            #region Intro Part

            var introNode = GetIntroNode(articleNode);
            
            if (introNode != null)
            {
                result.Append(introNode.InnerText + " ").Replace('\'', '"');
            }

            #endregion

            #region Article Content Part

            var nodes = GetAtricleNodes(articleNode);

            if (nodes == null)
            {
                return "Only Description" + GetDescriptionOnlyText(document);
            }
            
            result.Append(GrabText(nodes));

            #endregion

            return result.ToString();
        }

        #endregion

        #region Protected Methods

        protected String GrabText(HtmlNodeCollection nodes)
        {
            var result = new StringBuilder();

            for (var i = 0; i < nodes.Count; i++)
            {
                result.Append(nodes[i].InnerText + " ");
            }
            
            return CleanText(result.ToString());
        }

        protected abstract HtmlNode GetIntroNode(HtmlNode articleNode);

        protected abstract HtmlNodeCollection GetAtricleNodes(HtmlNode articleNode);

        protected abstract HtmlNode GetMainNode(HtmlDocument document);

        protected HtmlNode GetAlternateNode(HtmlDocument document)
        {
            return document.DocumentNode.SelectSingleNode("//meta[@name='description']");
        }

        protected String GetDescriptionOnlyText(HtmlDocument document)
        {
            var descriptionNode = GetAlternateNode(document);
            return descriptionNode.GetAttributeValue("content", "Not found");
        }

        #endregion

        #region Private Methods

        private static String CleanText(String article)
        {
            return HttpUtility.HtmlDecode(article).Replace('\'', '"');
        }

        #endregion

        #endregion
    }
}