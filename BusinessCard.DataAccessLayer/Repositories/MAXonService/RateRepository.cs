using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonService
{
    /// <inheritdoc cref="IRateRepository"/>
    internal class RateRepository : StandardRepository<Rate>, IRateRepository
    {
        public RateRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Rate>> GetRatesAsync(int serviceId)
        {
            const string sqlQuery = @"select Id,
		                                     Name,
                                             ServiceCounterId
                                      from Rates
                                      where ServiceId = @serviceId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return (await dbConnection.QueryAsync<Rate>(sqlQuery, new { serviceId })).ToArray();
        }

        /// <inheritdoc/>
        public async Task<Rate> GetRateByIdAsync(int id)
        {
            const string sqlQuery = @"select rts.Name,
                                             rts.Description,
		                                     rts.Price,
		                                     rts.IsSpecificPrice,
                                             rts.IsPublic
		                                     c.ConditionText,
                                             cv.Id,
		                                     cv.Value
                                      from Rates rts
                                        left join ConditionsValues cv
                                            on cv.RateId = rts.Id
                                        left join Conditions c
                                            on c.Id = cv.ConditionId
                                      where rts.Id = @id";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.QuerySingleAsync<Rate>(sqlQuery, new { id });
        }

        /// <inheritdoc/>
        public async Task<int> UpdateRateAsync(Rate rate)
        {
            const string sqlQuery = @"update Rates
                                      set Name = @Name, Description = @Description, Price = @Price, IsSpecificPrice = @IsSpecificPrice, IsPublic = @IsPublic
                                      where Id = @id";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.ExecuteAsync(sqlQuery, rate);
        }

        /// <inheritdoc/>
        public async Task<int> UpdateRateCalculatedValueAsync(int id, int? serviceCounterId)
        {
            const string sqlQuery = @"update Rates
                                      set ServiceCounterId = @serviceCounterId
                                      where Id = @id";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.ExecuteAsync(sqlQuery, new { id, serviceCounterId });
        }
    }
}