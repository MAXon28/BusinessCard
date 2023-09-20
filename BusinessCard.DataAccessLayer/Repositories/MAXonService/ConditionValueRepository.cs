using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonService
{
    /// <inheritdoc cref="IConditionValueRepository"/>
    internal class ConditionValueRepository : StandardRepository<ConditionValue>, IConditionValueRepository
    {
        public ConditionValueRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        /// <inheritdoc/>
        public async Task<int> AddConditionValues(IEnumerable<ConditionValue> conditionValues)
        {
            const string sqlQuery = @"insert ConditionsValues
                                      (Value, RateId, ConditionId)
                                      values
                                      (@Value, @RateId, @ConditionId)";
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.ExecuteAsync(sqlQuery, conditionValues);
        }

        /// <inheritdoc/>
        public async Task<int> UpdateConditionValues(IEnumerable<ConditionValue> conditionValues)
        {
            const string sqlQuery = @"update ConditionsValues
                                      set Value = @Value
                                      where Id = @Id";
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.ExecuteAsync(sqlQuery, conditionValues);
        }
    }
}