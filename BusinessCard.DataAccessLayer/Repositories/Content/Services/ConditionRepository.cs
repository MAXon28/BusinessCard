using BusinessCard.DataAccessLayer.Entities.Content.Services;
using BusinessCard.DataAccessLayer.Interfaces.Content.Services;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ConditionRepository : StandardRepository<Condition>, IConditionRepository
    {
        public ConditionRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}