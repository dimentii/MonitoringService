using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RssBusinessLogic.RSS
{
    [Serializable]
    public class FeedData
    {
        [XmlElement("title")]
        public String Title { get; set; }
        [XmlElement("description")]
        public String Description { get; set; }
        [XmlElement("pubDate")]
        public String PublishDate { get; set; }
        [XmlElement("link")]
        public String Link { get; set; }
        [XmlElement("item")]
        public List<FeedItem> Items { get; set; }
    }
}