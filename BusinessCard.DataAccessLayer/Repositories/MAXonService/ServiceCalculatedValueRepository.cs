using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using Dapper;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonService
{
    /// <inheritdoc cref="IServiceCalculatedValueRepository"/>
    internal class ServiceCalculatedValueRepository : StandardRepository<ServiceCalculatedValue>, IServiceCalculatedValueRepository
    {
        public ServiceCalculatedValueRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        /// <inheritdoc/>
        public async Task<ServiceCalculatedValue> GetCalculatedValueDataByRateAsync(int rateId)
        {
            const string sqlQuoery = @"SELECT	scv.Number,
		                                        scv.CalculatedValueId
                                        FROM ServicesCalculatedValues scv
	                                        INNER JOIN Rates r
	                                        ON r.ServiceCounterId = scv.Id
                                        WHERE r.Id = @rateId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.QuerySingleOrDefaultAsync<ServiceCalculatedValue>(sqlQuoery, new { rateId });
        }
    }
}