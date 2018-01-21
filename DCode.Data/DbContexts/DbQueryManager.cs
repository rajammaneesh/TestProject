using DCode.Models.Management;
using System;
using MySql.Data.MySqlClient;
using System.Configuration;
using DCode.Models.Common;

namespace DCode.Data.DbContexts
{
    public class DbQuueryManager : IDataManagement
    {
        public DbQuueryManager()
        {

        }

        public DatabaseTable RunQuery(string query)
        {
            DatabaseTable result = null;

            var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["AdoDCodeWebConString"].ConnectionString);

            var sqlCommand = new MySqlCommand(query, connection);

            try
            {
                connection.Open();

                var reader = sqlCommand.ExecuteReader();

                int record = 0;

                while (reader.Read())
                {
                    result = result ?? new DatabaseTable();

                    var currentRecord = new TableRow(record);

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var databaseColumn = new TableColumn
                        {
                            ColumnName = reader.GetName(i),
                            ColumnValue = Convert.ToString(reader[i])
                        };

                        currentRecord = currentRecord ?? new TableRow();

                        currentRecord.Column.Add(databaseColumn);
                    }

                    result.Table.Add(currentRecord);

                    record++;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                sqlCommand.Dispose();

                if (connection?.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                connection?.Dispose();
            }

            return result;
        }
    }
}
