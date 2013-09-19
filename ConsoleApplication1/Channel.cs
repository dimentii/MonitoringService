using System;
using System.Xml.Serialization;

namespace ConsoleApplication1
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
