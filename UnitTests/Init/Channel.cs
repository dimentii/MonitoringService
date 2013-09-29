using System;
using System.Xml.Serialization;

namespace UnitTests.Init
{
    [Serializable, XmlRoot("rss")]
    public class Channel
    {
        public Channel()
        {
            Data = null;
        }
        [XmlElement("channel")]
        public FeedData Data { get; set; }
    }
}