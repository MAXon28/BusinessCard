using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using Dapper;
using DapperAssistant;
using System.Data;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonService
{
    /// <inheritdoc cref="IConditionRepository"/>
    internal class ConditionRepository : StandardRepository<Condition>, IConditionRepository
    {
        public ConditionRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        /// <inheritdoc/>
        public async Task<bool> AddConditionAsync(Condition condition)
        {
            const string sqlProcedureName = "dbo.AddNewConditionToExistingConditionValues";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            dbConnection.Open();
            using var transaction = dbConnection.BeginTransaction();

            var conditionId = await AddAsync<int>(condition, dbConnection, transaction);

            var parameters = new DynamicParameters();
            parameters.Add("@ServiceId", condition.ServiceId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ConditionId", conditionId, DbType.Int32, ParameterDirection.Input);
            await dbConnection.ExecuteAsync(sqlProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

            transaction.Commit();

            return true;
        }
    }
}