using System;
using System.Xml.Serialization;
using HtmlAgilityPack;

namespace UnitTests.Init
{
    [Serializable]
    public class FeedItem
    {
        [XmlElement("title")]
        public String Title { get; set; }
        [XmlElement("description")]
        public String Description { get; set; }
        [XmlElement("pubDate")]
        public String PubDate { get; set; }
        [XmlElement("link")]
        public String Link { get; set; }
        [XmlElement("guid")]
        public String Guid { get; set; }
        public HtmlDocument Document { get; set; }
    }
}