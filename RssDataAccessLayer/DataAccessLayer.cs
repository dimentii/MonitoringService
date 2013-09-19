using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebsiteWorkers;

namespace RssDataAccessLayer
{
    public class DataAccessLayer
    {
        private const String ConnectionString = @"Server=dydewkipc\sqlexpress;Database=RSS;Trusted_Connection=True";

        // Fill database and return only new articles' links
        public async Task<List<String>> FillRssAsync(String sqlCommandString, GetArticleLink getArticleLink)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                await sqlConnection.OpenAsync();

                using (var sqlCommand = new SqlCommand(sqlCommandString, sqlConnection))
                {
                    using (var sqlReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        var listOfLinks = new List<String>();

                        while (await sqlReader.ReadAsync())
                        {
                            listOfLinks.Add(await getArticleLink(sqlReader));
                        }
                        return listOfLinks;
                    }
                }
            }
        }

        // Fill database with complete article's data and return number of successfully added data;
        public async Task<Int32> FillDataAsync(String sqlCommandString)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = sqlCommandString
                };

                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}
