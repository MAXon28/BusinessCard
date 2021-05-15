using Microsoft.Data.SqlClient;
using System.Data;

namespace BusinessCard.DataAccessLayer
{
    public class DbConnectionKeeper
    {
        private readonly string _connectionString;

        public DbConnectionKeeper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetDbConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}