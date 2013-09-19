using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConsoleApplication1
{
    //[XmlElement("")]
    [Serializable]
    public class FeedData
    {
        public FeedData()
        {
            Title = String.Empty;
            Description = String.Empty;
            PubDate = String.Empty;
            Items = null;
        }
        [XmlElement("title")]
        public String Title { get; set; }
        [XmlElement("description")]
        public String Description { get; set; }
        [XmlElement("pubDate")]
        public String PubDate { get; set; }
        [XmlElement("link")]
        public String Link { get; set; }
        [XmlElement("item")]
        public List<FeedItem> Items { get; set; }
    }
}