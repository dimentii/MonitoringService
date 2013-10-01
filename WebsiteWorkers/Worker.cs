﻿using System;
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

        public CompleteArticleData GetData(HtmlDocument document)
        {
            #region Get Description

            var description = GetDescriptionText(document);

            #endregion

            #region Get Article Text

            var articleText = new StringBuilder();

            var articleNode = GetMainNode(document);

            if (articleNode != null)
            {
                #region Intro Part

                var introNode = GetIntroNode(articleNode);

                if (introNode != null)
                {
                    articleText.Append(introNode.InnerText + " ").Replace('\'', '"');
                }

                #endregion

                #region Article Content Part

                var nodes = GetAtricleNodes(articleNode);

                articleText.Append(GrabArticleText(nodes));

                #endregion
            }

            #endregion

            #region Get Author

            var author = GetAuthor(document);

            #endregion

            #region Get Title

            var title = GetTitle(document);

            #endregion

            return new CompleteArticleData
                {
                    Author = author,
                    Title = title,
                    Description = description,
                    Article = articleText.ToString()
                };
        }

        #endregion

        #region Protected Methods

        #region Article part

        protected abstract HtmlNode GetMainNode(HtmlDocument document);

        protected abstract HtmlNode GetIntroNode(HtmlNode articleNode);

        protected abstract HtmlNodeCollection GetAtricleNodes(HtmlNode articleNode);

        protected String GrabArticleText(HtmlNodeCollection nodes)
        {
            if (nodes == null)
            {
                return String.Empty;
            }

            var result = new StringBuilder();

            for (var i = 0; i < nodes.Count; i++)
            {
                result.Append(nodes[i].InnerText + " ");
            }

            return CleanText(result.ToString());
        }

        #endregion

        protected abstract String GetAuthor(HtmlDocument document);

        #region Description part

        protected virtual HtmlNode GetDescriptionNode(HtmlDocument document)
        {
            return document.DocumentNode.SelectSingleNode("//meta[@property='og:description']");
        }

        protected String GetDescriptionText(HtmlDocument document)
        {
            var descriptionNode = GetDescriptionNode(document);
            if (descriptionNode == null)
                return String.Empty;
            var descriptionText = descriptionNode.GetAttributeValue("content", String.Empty);
            return CleanText(descriptionText);
        }

        #endregion

        #region Title part

        protected String GetTitle(HtmlDocument document)
        {
            var titleNode = GetTitleNode(document);
            if (titleNode == null)
                return String.Empty;
            var titleText = titleNode.GetAttributeValue("content", String.Empty);
            return CleanText(titleText);
        }

        protected HtmlNode GetTitleNode(HtmlDocument document)
        {
            return document.DocumentNode.SelectSingleNode("//meta[@property='og:title']");
        }

        #endregion
        
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