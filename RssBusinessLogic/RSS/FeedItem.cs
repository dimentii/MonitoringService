using System;
using System.Xml.Serialization;

namespace RssBusinessLogic.RSS
{
    [Serializable]
    public class FeedItem
    {
        private String _title;
        private String _description;
        private String _publishDate;
        private String _link;
        private String _guid;

        [XmlElement("title")]
        public String Title
        {
            get { return _title; }
            set { _title = value.Replace('\'', '\"'); }
        }

        [XmlElement("description")]
        public String Description
        {
            get { return _description; }
            set { _description = value.Replace('\'', '\"'); }
        }

        [XmlElement("pubDate")]
        public String PublishDate
        {
            get { return _publishDate; }
            set { _publishDate = value.Replace('\'', '\"'); }
        }

        [XmlElement("link")]
        public String Link
        {
            get { return _link; }
            set { _link = value.Replace('\'', '\"'); }
        }

        [XmlElement("guid")]
        public String Guid
        {
            get { return _guid; }
            set { _guid = value.Replace('\'', '\"'); }
        }
    }
}