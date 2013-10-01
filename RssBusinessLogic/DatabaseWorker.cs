﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using RssBusinessLogic.Interfaces;
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

            try
            {
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
                var list = await _dataAccessLayer.FillRssAsync(sqlCommandString, (Int32)dbWorker.Identifier);
                return list.ToList();
            }
            catch (Exception exception)
            {
                Logger logger = LogManager.GetCurrentClassLogger();

                logger.Error("Error while writing to rss database for {0}. Error message: {1}", table, exception.Message);

                return null;
            }
        }

        // Create sql query which fills database with complete articles and additional data
        public async Task<Int32> FillDbWithCompleteArticleAsync(String table, CompleteArticleData[] data)
        {
            if (data == null || data.Length == 0)
            {
                return 0;
            }

            try
            {
                var sqlCommandString =
                    String.Format(
                        "use final " +
                        "if object_id('[dbo].[{0}]') is null " +
                        "create table {0} " +
                        "(" +
                        "ID	int	not null identity(1,1), " +
                        "Link nvarchar(200), " +
                        "Author nvarchar(100), " +
                        "Title nvarchar(500), " +
                        "Description nvarchar(max), " +
                        "Article nvarchar(max), " +
                        "Date datetime not null default getdate(), " +
                        "constraint PK_{0} " +
                        "Primary Key(ID)" +
                        ") " +
                        "insert into [dbo].[{0}] " +
                        "(Link, Author, Title, Description, Article) " +
                        "values {1}",
                        table, ConvertAtricleDataToString(data));
                return await _dataAccessLayer.FillCompleteDataAsync(sqlCommandString);
            }
            catch (Exception exception)
            {
                Logger logger = LogManager.GetCurrentClassLogger();

                logger.Error("Error while writing to final database for {0}. Error message: {1}", table, exception.Message);

                return 0;
            }
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
                stringBuilder.AppendFormat(article.ToString());
            }
            stringBuilder.Length--;
            return stringBuilder.ToString();
        }

        #endregion

        #endregion
    }
}