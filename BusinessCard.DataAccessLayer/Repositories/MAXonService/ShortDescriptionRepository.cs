using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonService
{
    /// <inheritdoc cref="IShortDescriptionRepository"/>
    internal class ShortDescriptionRepository : StandardRepository<ShortDescription>, IShortDescriptionRepository
    {
        public ShortDescriptionRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        /// <inheritdoc/>
        public async Task<int> AddShortDescriptionsAsync(IReadOnlyCollection<ShortDescription> shortDescriptions)
        {
            const string sqlQuery = @"insert ServiceShortDescriptions
                                      (Data, ServiceId)
                                      values
                                      (@Data, @ServiceId)";
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.ExecuteAsync(sqlQuery, shortDescriptions);
        }

        /// <inheritdoc/>
        public async Task<int> UpdateShortDescriptionsAsync(IReadOnlyCollection<ShortDescription> shortDescriptions)
        {
            const string sqlQuery = @"update ServiceShortDescriptions
                                      set Data = @Data
                                      where Id = @Id";
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.ExecuteAsync(sqlQuery, shortDescriptions);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ShortDescription>> GetShortDescriptionsForPublicServicesAsync()
        {
            const string sqlQuery = @"select * from ServiceShortDescriptions
                                      where ServiceId in (select Id from Services
                                                          where IsPublic = 1)";
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.QueryAsync<ShortDescription>(sqlQuery);
        }
    }
}