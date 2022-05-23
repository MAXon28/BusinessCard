using BusinessCard.DataAccessLayer.Entities.Content;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.Content
{
    /// <summary>
    /// 
    /// </summary>
    public class RuleRepository : StandardRepository<Rule>, IRuleRepository
    {
        public RuleRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}