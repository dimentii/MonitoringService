using System;
using System.Text;
using System.Xml.Serialization;

namespace RssBusinessLogic.RSS
{
    [Serializable, XmlRoot("rss")]
    public class Channel
    {
        [XmlElement("channel")]
        public FeedData Data { get; set; }

        public String ToString(String num)
        {
            var result = new StringBuilder();
            var str = "(N'{0}',N'{1}',N'{2}',N'{" + num + "}'),";
            foreach (var item in Data.Items)
            {
                result.AppendFormat(str, item.Title, item.Description, item.PublishDate, item.Guid, item.Link);
            }
            result.Length--;
            return result.ToString();
        }
    }
}