using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCourses.Models.Exceptions;
using MyCourses.Models.Options;
using MyCourses.Models.ValueTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MyCourses.Models.Services.Infrastructure
{
    public class SQLDBAccessor : IDatabaseAccessor
    {
        private readonly ILogger<SQLDBAccessor> logger;
        private readonly IOptionsMonitor<ConnectionStringsOptions> connectionStringOptions;

        public SQLDBAccessor(ILogger<SQLDBAccessor> logger, IOptionsMonitor<ConnectionStringsOptions> connectionStringOptions)
        {
            this.logger = logger;
            this.connectionStringOptions = connectionStringOptions;
        }

        public async Task<DataSet> QueryAsync(FormattableString formattableQuery)
        {

            using SqlConnection conn = await GetOpenedConnection();
            using SqlCommand cmd = GetCommand(formattableQuery, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            var dataSet = new DataSet();

            //Creiamo tanti DataTable per quante sono le tabelle
            //di risultati trovate dal SqlDataReader
            do
            {
                var dataTable = new DataTable();
                dataSet.Tables.Add(dataTable);
                dataTable.Load(reader);
            } while (!reader.IsClosed);

            return dataSet;
        }


        private static SqlCommand GetCommand(FormattableString formattableQuery, SqlConnection conn)
        {
            //Creiamo dei SqlParameter a partire dalla FormattableString
            var queryArguments = formattableQuery.GetArguments();
            var sqlParameters = new List<SqlParameter>();
            for (var i = 0; i < queryArguments.Length; i++)
            {
                if (queryArguments[i] is Sql)
                {
                    continue;
                }

                var parameter = new SqlParameter(i.ToString(),value: queryArguments[i] ?? DBNull.Value);
                sqlParameters.Add(parameter);
                //queryArguments[i] = "@" + i;
            }

            string query = formattableQuery.ToString();
            var cmd = new SqlCommand(query, conn);
            //Aggiungiamo i SqliteParameters al SqlCommand
            cmd.Parameters.AddRange(sqlParameters.ToArray());
            //Inviamo la query al database e otteniamo un SqlDataReader
            //per leggere i risultati
            return cmd;
        }
        
        public async Task<int> CommandAsync(FormattableString formattableCommand)
        {
            try
            {
                using SqlConnection conn = await GetOpenedConnection();
                using SqlCommand cmd = GetCommand(formattableCommand, conn);
                int affectedRows = await cmd.ExecuteNonQueryAsync();
                return affectedRows;
            }
            catch (SqlException e) when (e.Number == 2601)
            {
                throw new ConstraintViolationException(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public async Task<T> QueryScalarAsync<T>(FormattableString formattableQuery)
        {
            using SqlConnection conn = await GetOpenedConnection();
            using SqlCommand cmd = GetCommand(formattableQuery, conn);
            object result = await cmd.ExecuteScalarAsync();
            return (T)Convert.ChangeType(result, typeof(T));
        }

        private async Task<SqlConnection> GetOpenedConnection()
        {
            string connectionString = connectionStringOptions.CurrentValue.Default;

            var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();
            return conn;
        }
    }
}