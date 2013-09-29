using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace UnitTests.Init
{
    class RssLoader
    {
        public List<FeedItem> GetRssData(String rssLink)
        {
            try
            {
                using (var xmlReader = XmlReader.Create(rssLink))
                {
                    var xmlSerializer = new XmlSerializer(typeof(Channel));
                    var rssChannel = (Channel)xmlSerializer.Deserialize(xmlReader);
                    return rssChannel.Data.Items;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
