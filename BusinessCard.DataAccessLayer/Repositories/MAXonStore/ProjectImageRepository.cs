using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    internal class ProjectImageRepository : StandardRepository<ProjectImage>, IProjectImageRepository
    {
        public ProjectImageRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}