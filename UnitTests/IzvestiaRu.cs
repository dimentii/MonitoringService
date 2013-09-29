using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Init;
using WebsiteWorkers;

namespace UnitTests
{
    [TestClass]
    public class IzvestiaRu
    {
        private List<FeedItem> _articles;

        private readonly Worker _worker = new WebsiteWorkers.Workers.IzvestiaRu(Identifier.Link);

        private const String RssLink = "http://izvestia.ru/xml/rss/all.xml";

        [TestInitialize]
        public void Initialize()
        {
            var loader = new RssLoader();
            _articles = loader.GetRssData(RssLink);

            var docLoader = new DocumentsLoader();
            docLoader.LoadDocuments(_articles);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _articles = null;
        }

        [TestMethod]
        public void GetMainArticleNode()
        {
            var privateObject = new PrivateObject(_worker);
            var nodes = new List<HtmlNode>();
            foreach (var item in _articles)
            {
                var articleNode = privateObject.Invoke("GetMainNode", item.Document);
                if (articleNode as HtmlNode != null)
                    nodes.Add(articleNode as HtmlNode);
            }
            Assert.AreEqual(_articles.Count, nodes.Count);
        }

        [TestMethod]
        public void GetDescription()
        {
            var privateObject = new PrivateObject(_worker);
            var nodes = new List<String>();
            foreach (var item in _articles)
            {
                var descriptionNode = privateObject.Invoke("GetDescriptionOnlyText", item.Document) as String;
                if (descriptionNode != null && descriptionNode != "Not found")
                    nodes.Add(descriptionNode);
            }
            Assert.AreEqual(_articles.Count, nodes.Count);
        }

        [TestMethod]
        public void GetArticleNodes()
        {
            var privateObject = new PrivateObject(_worker);
            var nodes = new List<HtmlNodeCollection>();
            foreach (var item in _articles)
            {
                var articleNode = privateObject.Invoke("GetMainNode", item.Document) as HtmlNode;
                if (articleNode == null) 
                    continue;
                var nodesCollection = privateObject.Invoke("GetAtricleNodes", articleNode) as HtmlNodeCollection;
                    
                if (nodesCollection != null && nodesCollection.Count > 0)
                {
                    nodes.Add(nodesCollection);
                }
            }
            Assert.AreEqual(_articles.Count, nodes.Count);
        }
    }
}