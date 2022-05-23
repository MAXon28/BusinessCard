using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectCompatibilityRepository : StandardRepository<ProjectCompatibility>, IProjectCompatibilityRepository
    {
        public ProjectCompatibilityRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}