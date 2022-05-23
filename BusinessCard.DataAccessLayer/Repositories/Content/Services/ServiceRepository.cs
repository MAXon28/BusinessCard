using BusinessCard.DataAccessLayer.Entities.Content.Services;
using BusinessCard.DataAccessLayer.Interfaces.Content.Services;
using Dapper;
using DapperAssistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceRepository : StandardRepository<Service>, IServiceRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly DbConnectionKeeper _dbConnectionKeeper;

        public ServiceRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) => _dbConnectionKeeper = dbConnectionKeeper;

        public Task<List<Service>> GetAllServicesWithShortDescriptions()
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> GetFullDescriptionByServiceId(int id)
        {
            const string sqlQuery = @"SELECT FullDescription
                                      FROM Services
                                      WHERE Id = @id";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QuerySingleAsync<string>(sqlQuery, new { id });
        }

        public async Task<Service> GetService(int serviceId)
        {
            const string sqlQuery = @"SELECT s.Id,
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
                                      FROM Services s
                                      LEFT JOIN Rates rts
                                      ON s.Id = rts.ServiceId
                                      LEFT JOIN ConditionsValues cv
                                      ON rts.Id = cv.RateId
                                      LEFT JOIN Conditions c
                                      ON c.Id = cv.ConditionId
                                      WHERE s.Id = @serviceId
                                      ORDER BY rts.Price, c.Priority";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            Console.WriteLine(sqlQuery);

            var serviceDictionary = new Dictionary<int, Service>();

            var service = (await dbConnection.QueryAsync<Service, Rate, Condition, ConditionValue, Service>(
                    sqlQuery,
                    (service, rate, condition, conditionValue) =>
                    {
                        if (!serviceDictionary.TryGetValue(service.Id, out var serviceInDictionary))
                        {
                            serviceInDictionary = service;
                            serviceInDictionary.Rates = new Dictionary<Guid, Rate>();
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