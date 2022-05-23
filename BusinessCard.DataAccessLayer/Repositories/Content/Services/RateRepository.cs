using BusinessCard.DataAccessLayer.Entities.Content.Services;
using BusinessCard.DataAccessLayer.Interfaces.Content.Services;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class RateRepository : StandardRepository<Rate>, IRateRepository
    {
        public RateRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}