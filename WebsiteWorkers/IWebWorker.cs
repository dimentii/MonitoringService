using System;
using HtmlAgilityPack;

namespace WebsiteWorkers
{
    public interface IWebWorker
    {
        String GetText(HtmlDocument document);
    }
}
