using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Comment_Post.Extensions
{
    public static class DapperExtensions
    {
        public static async Task<IEnumerable<T>> QueryAsync<T>(this IDbConnection connection, string sql, object parameters = null)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            return await connection.QueryAsync<T>(sql, parameters);
        }

        public static async Task<int> ExecuteAsync(this IDbConnection connection, string sql, object parameters = null)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            return await connection.ExecuteAsync(sql, parameters);
        }

        public static async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(this IDbConnection connection, string storedProcedureName, object parameters = null)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            return await connection.QueryAsync<T>(storedProcedureName, parameters, commandType: 
                CommandType.StoredProcedure);
        }

       
    }
}
