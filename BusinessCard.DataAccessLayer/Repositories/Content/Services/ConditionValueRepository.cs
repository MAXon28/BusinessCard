using BusinessCard.DataAccessLayer.Entities.Content.Services;
using BusinessCard.DataAccessLayer.Interfaces.Content.Services;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ConditionValueRepository : StandardRepository<ConditionValue>, IConditionValueRepository
    {
        public ConditionValueRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}