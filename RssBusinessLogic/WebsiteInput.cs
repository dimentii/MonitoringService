using System;
using WebsiteWorkers;

namespace RssBusinessLogic
{
    public class WebsiteInput
    {
        public WebsiteInput(String newspaper, String xmlLink, Worker worker)
        {
            Newspaper = newspaper;
            XmlLink = xmlLink;
            Worker = worker;
        }

        public String XmlLink { get; set; }
        public String Newspaper { get; set; }
        public Worker Worker { get; set; }
    }
}
