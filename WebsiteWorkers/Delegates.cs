using System;
using System.Data.Common;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WebsiteWorkers
{
    // Every website's parser should has this methods

    public delegate Task<String> GetArticleLink(DbDataReader reader);
    
    public delegate String GetArticleText(HtmlDocument document);
}
