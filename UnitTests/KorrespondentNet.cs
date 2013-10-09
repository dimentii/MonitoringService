using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Init;
using WebsiteWorkers;

namespace UnitTests
{
    [TestClass]
    public class KorrespondentNet
    {
        private static List<FeedItem> _articles;

        private static readonly Worker Worker = new WebsiteWorkers.Workers.KorrespondentNet();

        private const String RssLink = "http://k.img.com.ua/rss/ru/news.xml";

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            var loader = new RssLoader();
            _articles = loader.GetRssData(RssLink);

            var docLoader = new DocumentsLoader(new HttpClient());
            _articles = docLoader.LoadDocuments(_articles, Worker);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            _articles = null;
        }

        [TestMethod]
        public void GetMainArticleNode()
        {
            var privateObject = new PrivateObject(Worker);
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
            var privateObject = new PrivateObject(Worker);
            var nodes = new List<String>();
            foreach (var item in _articles)
            {
                var descriptionNode = privateObject.Invoke("GetDescriptionText", item.Document) as String;
                if (!String.IsNullOrEmpty(descriptionNode))
                    nodes.Add(descriptionNode);
            }
            Assert.AreEqual(_articles.Count, nodes.Count);
        }

        [TestMethod]
        public void GetArticleNodes()
        {
            var privateObject = new PrivateObject(Worker);
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

        [TestMethod]
        public void GetAuthor()
        {
            var privateObject = new PrivateObject(Worker);
            var nodes = new List<String>();
            foreach (var item in _articles)
            {
                var descriptionNode = privateObject.Invoke("GetAuthor", item.Document) as String;
                if (!String.IsNullOrEmpty(descriptionNode))
                    nodes.Add(descriptionNode);
            }
            Assert.AreEqual(0, nodes.Count);
        }

        [TestMethod]
        public void GetTitle()
        {
            var privateObject = new PrivateObject(Worker);
            var nodes = new List<String>();
            foreach (var item in _articles)
            {
                var descriptionNode = privateObject.Invoke("GetTitle", item.Document) as String;
                if (!String.IsNullOrEmpty(descriptionNode))
                    nodes.Add(descriptionNode);
            }
            Assert.AreEqual(_articles.Count, nodes.Count);
        }

        [TestMethod]
        public void GetIdentifiers()
        {
            var uniques = new List<String>();
            if (Worker.LinkContainer == RssLinkContainer.Link)
            {
                foreach (var link in _articles)
                {
                    if (!uniques.Contains(link.Link))
                    {
                        uniques.Add(link.Link);
                    }
                }
            }
            else
            {
                foreach (var link in _articles)
                {
                    if (!uniques.Contains(link.Guid))
                    {
                        uniques.Add(link.Guid);
                    }
                }
            }
            Assert.AreEqual(_articles.Count, uniques.Count);
        }

        [TestMethod]
        public void GetArticleText()
        {
            var completeArticles = new List<CompleteArticleData>();
            foreach (var article in _articles)
            {
                completeArticles.Add(Worker.GetData(article.Document));
            }

            var nodes = new List<String>();
            foreach (var article in completeArticles)
            {
                if (!String.IsNullOrEmpty(article.Article))
                    nodes.Add(article.Article);
            }

            Assert.AreEqual(_articles.Count, nodes.Count);
        }

        [TestMethod]
        public void CheckEncoding()
        {
            var completeArticles = new List<CompleteArticleData>();
            foreach (var article in _articles)
            {
                completeArticles.Add(Worker.GetData(article.Document));
            }

            var encoding = Encoding.UTF8;
            var articleForCheck = completeArticles.FirstOrDefault();
            if (articleForCheck != null)
            {
                var bytes = encoding.GetBytes(articleForCheck.Article);
                var max = bytes.Max();
                var min = bytes.Min();
                Assert.IsTrue(max > min);
            }
            else
                Assert.Fail("Did not get any article");
        }
    }
}