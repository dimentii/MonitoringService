using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using RssBusinessLogic.Interfaces;
using RssBusinessLogic.RSS;
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
        public async Task<List<String>> FillDbWithRssAsync(String table, Channel channel, IDbWorker dbWorker)
        {
            if (channel == null || channel.Data == null)
                return null;

            try
            {
                var sqlCommandString =
                    String.Format(
                        "use RssTest " +
                        "if object_id('[dbo].[{1}]') is null " +
                        "create table {1} " +
                        "(" +
                        "ID int not null identity(1,1), " +
                        "Title nvarchar(max), " +
                        "Descript nvarchar(max), " +
                        "PublishDate nvarchar(100), " +
                        "Link nvarchar(200), " +
                        "constraint PK_{1} " +
                        "Primary Key(ID)" +
                        ") " +
                        "declare @bufferTable Table " +
                        "(Title nvarchar(max), " +
                        "Descript nvarchar(max), " +
                        "PublishDate nvarchar(100), " +
                        "Link nvarchar(200))" +
                        "insert into @bufferTable " +
                        "(Title, Descript, PublishDate, Link) " +
                        "values {0}" +
                        "insert into [dbo].[{1}] (Title, Descript, PublishDate, Link) " +
                        "output inserted.Link " +
                        "select * " +
                        "from @bufferTable as buf " +
                        "where not exists " +
                        "(select * " +
                        "from [dbo].[{1}] " +
                        "where [Link] = buf.Link)",
                        channel.ToString(((Int32)dbWorker.LinkContainer).ToString(CultureInfo.InvariantCulture)), table);
                
                var list = await _dataAccessLayer.FillRssAsync(sqlCommandString);
                
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
                        "use FinalTest " +
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

        public async Task RemoveUnhandledLinks(String table, String link)
        {
            try
            {
                var sqlCommandString = String.Format("use RssTest delete from [dbo].[{0}] where [Link] = '{1}'", table, link);
                await _dataAccessLayer.RemoveUnhandledArticlesAsync(sqlCommandString);
            }
            catch (Exception exception)
            {
                Logger logger = LogManager.GetCurrentClassLogger();

                logger.Error("Error while removing from rss database from table {0}. Error message: {1}", table, exception.Message);
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