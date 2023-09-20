using BusinessCard.DataAccessLayer.Entities.Content;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.Content
{
    /// <summary>
    /// 
    /// </summary>
    internal class SkillRepository : StandardRepository<Skill>, ISkillRepository
    {
        public SkillRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}