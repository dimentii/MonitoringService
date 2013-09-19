using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ConsoleApplication1
{
    public class FeedItem
    {
        [XmlElement("title")]
        public String Title { get; set; }
        [XmlElement("description")]
        public String Description { get; set; }
        [XmlElement("pubDate")]
        public String PubDate { get; set; }
        [XmlElement("link")]
        public List<String> Link { get; set; }
        [XmlElement("guid")]
        public String Guid { get; set; }
    }
}