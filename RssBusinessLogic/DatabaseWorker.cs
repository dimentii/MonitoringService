using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RssDataAccessLayer;
using WebsiteWorkers;

namespace RssBusinessLogic
{
    internal class DatabaseWorker
    {
        // IoC
        private readonly DataAccessLayer _dataAccessLayer = new DataAccessLayer();

        // Make sql query which checks if input data is already in table and return List of article's links 
        public async Task<List<String>> FillDbWithRssAsync(String table, String xmlRss, GetArticleLink getArticleLink)
        {
            if (xmlRss == null)
                return null;
            var sqlCommandString =
                String.Format(
                    "use rss if object_id('[dbo].[{1}]') is null create table {1} (ID int not null identity(1,1), Title nvarchar(max), Descript nvarchar(max), PublishDate nvarchar(100), Guid_link nvarchar(200), Link nvarchar(200), constraint PK_{1} Primary Key(ID)) declare @Xml xml set @Xml = '{0}' insert into [dbo].[{1}] (Title, Descript, PublishDate, Guid_link, Link) output inserted.Guid_link, inserted.Link, inserted.Descript select Item.value('title[1]', 'nvarchar(max)'), Item.value('description[1]', 'nvarchar(max)'), Item.value('pubDate[1]', 'nvarchar(100)'), Item.value('guid[1]', 'nvarchar(200)'), Item.value('link[1]', 'nvarchar(200)') from @Xml.nodes('channel/item') as Result(Item) where not exists (select * from [dbo].[{1}] where ([Guid_link] = Item.value('guid[1]', 'nvarchar(200)') or [Guid_link] is null) and (Link = Item.value('link[1]', 'nvarchar(200)')) or Link is null)",
                    xmlRss.Replace('\'', '\"'), table);
            var list = await _dataAccessLayer.FillRssAsync(sqlCommandString, getArticleLink);
            return list.ToList();
        }
        
        // Create sql query which fills database with complete articles and additional data
        public async Task<Int32> FillDbWithCompleteArticleAsync(String table, CompleteArticleData[] data)
        {
            if (data == null)
            {
                Console.WriteLine("No new articles");
                return 0;
            }
            var sqlCommandString = String.Format("use final if object_id('[dbo].[{0}]') is null create table {0} (ID	int	not null identity(1,1), Link nvarchar(200), Article nvarchar(max) constraint PK_{0} Primary Key(ID)) insert into [dbo].[{0}] (Link, Article) values {1}", table, ConvertAtricleDataToString(data));
            return await _dataAccessLayer.FillDataAsync(sqlCommandString);
        }

        // Helper to build query
        private static String ConvertAtricleDataToString(IEnumerable<CompleteArticleData> articles)
        {
            var stringBuilder = new StringBuilder();
            foreach (var article in articles)
            {
                if (article == null)
                    continue;
                stringBuilder.AppendFormat("(N'{0}', N'{1}'),", article.Link, article.Article);
            }
            stringBuilder.Length--;
            return stringBuilder.ToString();
        }
    }
}