using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RssBusinessLogic.Interfaces;
using RssDataAccessLayer;
using RssDataAccessLayer.Interfaces;
using WebsiteWorkers;

namespace RssBusinessLogic
{
    class DatabaseWorker : IDatabaseWorker
    {
        #region Fields

        private readonly IDataAccessLayer _dataAccessLayer;

        #endregion

        #region Constructors

        public DatabaseWorker(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        #endregion

        #region Methods

        #region Public

        // Make sql query which checks if input data is already in table and return List of article's links 
        public async Task<List<String>> FillDbWithRssAsync(String table, String xmlRss, IDbWorker dbWorker)
        {
            if (xmlRss == null)
                return null;
            var sqlCommandString =
                String.Format(
                    "use rss " +
                    "if object_id('[dbo].[{1}]') is null " +
                    "create table {1} " +
                    "(" +
                    "ID int not null identity(1,1), " +
                    "Title nvarchar(max), " +
                    "Descript nvarchar(max), " +
                    "PublishDate nvarchar(100), " +
                    "Guid_link nvarchar(200), " +
                    "Link nvarchar(200), " +
                    "constraint PK_{1} " +
                    "Primary Key(ID)" +
                    ") " +
                    "declare @Xml xml set @Xml = '{0}'" +
                    " insert into [dbo].[{1}] (Title, Descript, PublishDate, Guid_link, Link) " +
                    "output inserted.Guid_link, inserted.Link, inserted.Descript " +
                    "select Item.value('title[1]', 'nvarchar(max)'), " +
                    "Item.value('description[1]', 'nvarchar(max)'), " +
                    "Item.value('pubDate[1]', 'nvarchar(100)'), " +
                    "Item.value('guid[1]', 'nvarchar(200)'), " +
                    "Item.value('link[1]', 'nvarchar(200)') " +
                    "from @Xml.nodes('channel/item') as Result(Item) " +
                    "where not exists " +
                    "(select * " +
                    "from [dbo].[{1}] " +
                    "where {2})",
                    xmlRss.Replace('\'', '\"'), table, dbWorker.GetIdentifyingQuery());
            Debugger.Break();
            var list = await _dataAccessLayer.FillRssAsync(sqlCommandString, dbWorker.GetLink);
            Debugger.Break();
            return list.ToList();
        }

        // Create sql query which fills database with complete articles and additional data
        public async Task<Int32> FillDbWithCompleteArticleAsync(String table, CompleteArticleData[] data)
        {
            if (data == null)
            {
                return 0;
            }
            var sqlCommandString =
                String.Format(
                    "use final " +
                    "if object_id('[dbo].[{0}]') is null " +
                    "create table {0} " +
                    "(" +
                    "ID	int	not null identity(1,1), " +
                    "Link nvarchar(200), " +
                    "Article nvarchar(max) " +
                    "constraint PK_{0} " +
                    "Primary Key(ID)" +
                    ") " +
                    "insert into [dbo].[{0}] " +
                    "(Link, Article) " +
                    "values {1}",
                    table, ConvertAtricleDataToString(data));
            return await _dataAccessLayer.FillDataAsync(sqlCommandString);
        }

        #endregion

        #region Private

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

        #endregion

        #endregion
    }
}