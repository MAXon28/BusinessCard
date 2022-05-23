using BusinessCard.DataAccessLayer.Entities.Content;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.Content
{
    /// <summary>
    /// 
    /// </summary>
    public class ExperienceRepsoitory : StandardRepository<Experience>, IExperienceRepository
    {
        public ExperienceRepsoitory (DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}