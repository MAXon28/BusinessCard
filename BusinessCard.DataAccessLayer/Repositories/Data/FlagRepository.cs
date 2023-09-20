using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using Dapper;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.Data
{
    /// <inheritdoc cref="IFlagRepository"/>
    public class FlagRepository : StandardRepository<Flag>, IFlagRepository
    {
        public FlagRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        /// <inheritdoc/>
        public async Task<bool> GetFlagValueAsync(string key)
        {
            const string sqlQuery = @"select Value from Flags
                                      where FlagKey = @key";
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.QuerySingleAsync<bool>(sqlQuery, new { key });
        }

        /// <inheritdoc/>
        public async Task<int> UpdateFlagValueAsync(Flag flag)
        {
            const string sqlQuery = @"update Flags
                                      set Value = @newValue
                                      where FlagKey = @key";
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.ExecuteAsync(sqlQuery, new 
            {
                NewValue = flag.Value,
                Key = flag.FlagKey
            });
        }
    }
}