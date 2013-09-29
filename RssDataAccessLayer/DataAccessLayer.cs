using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using RssDataAccessLayer.Interfaces;

namespace RssDataAccessLayer
{
    public class DataAccessLayer : IDataAccessLayer
    {
        #region Fields

        private const String ConnectionString = @"Server=dydewkipc\sqlexpress;Database=RSS;Trusted_Connection=True";

        #endregion

        #region Methods

        #region Public

        // Fill database and return only new articles' links
        public async Task<List<String>> FillRssAsync(String sqlCommandString, Int32 columnNumber)
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
                            listOfLinks.Add(await GetArticleLink(sqlReader, columnNumber));
                        }
                        return listOfLinks;
                    }
                }
            }
        }

        // Fill database with complete article's data and return number of successfully added data;
        public async Task<Int32> FillCompleteDataAsync(String sqlCommandString)
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

        #endregion

        #region Private

        private async Task<String> GetArticleLink(SqlDataReader reader, Int32 columnNumber)
        {
            return await reader.GetFieldValueAsync<String>(columnNumber);
        }

        #endregion

        #endregion
    }
}
