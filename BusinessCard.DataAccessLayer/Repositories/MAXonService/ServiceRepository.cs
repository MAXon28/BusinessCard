using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonService
{
    /// <inheritdoc cref="IServiceRepository"/>
    internal class ServiceRepository : StandardRepository<Service>, IServiceRepository
    {
        public ServiceRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        /// <inheritdoc/>
        public async Task<string> GetFullDescriptionByServiceId(int id)
        {
            const string sqlQuery = @"SELECT FullDescription
                                      FROM Services
                                      WHERE Id = @id";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QuerySingleAsync<string>(sqlQuery, new { id });
        }

        /// <inheritdoc/>
        public async Task<Service> GetService(int serviceId)
        {
            const string sqlQuery = @"select s.Id,
                                             s.Name,
                                             s.Price,
                                             s.ConcretePrice,
                                             s.NeedTechnicalSpecification,
		                                     s.PrePrice,
		                                     s.PreDeadline,
		                                     rts.Id,
		                                     rts.Name,
                                             rts.Description,
		                                     rts.Price,
		                                     rts.IsSpecificPrice,
                                             c.Id,
		                                     c.ConditionText,
                                             cv.Id,
		                                     cv.Value
                                      from Services s
                                        left join Rates rts
                                            on rts.ServiceId = s.Id and rts.IsPublic = 1
                                        left join ConditionsValues cv
                                            on cv.RateId = rts.Id
                                        left join Conditions c
                                            on c.Id = cv.ConditionId
                                      where s.Id = @serviceId
                                      order by rts.Price";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            var serviceDictionary = new Dictionary<int, Service>();

            var service = (await dbConnection.QueryAsync<Service, Rate, Condition, ConditionValue, Service>(
                    sqlQuery,
                    (service, rate, condition, conditionValue) =>
                    {
                        if (!serviceDictionary.TryGetValue(service.Id, out var serviceInDictionary))
                        {
                            serviceInDictionary = service;
                            serviceInDictionary.Rates = new Dictionary<int, Rate>();
                            serviceDictionary.Add(serviceInDictionary.Id, serviceInDictionary);
                        }

                        if (rate is null)
                            return serviceInDictionary;

                        if (!serviceInDictionary.Rates.TryGetValue(rate.Id, out var rateInDictionary))
                        {
                            rateInDictionary = rate;
                            rateInDictionary.ConditionsValues = new List<ConditionValue>();
                            serviceInDictionary.Rates.Add(rateInDictionary.Id, rateInDictionary);
                        }

                        conditionValue.Condition = condition;
                        rateInDictionary.ConditionsValues.Add(conditionValue);
                        return serviceInDictionary;
                    },
                    new { serviceId },
                    splitOn: "Id,Id,Id,Id"))
            .Distinct()
            .FirstOrDefault();

            return service;
        }
    }
}