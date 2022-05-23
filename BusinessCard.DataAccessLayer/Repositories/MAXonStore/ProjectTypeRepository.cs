using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectTypeRepository : StandardRepository<ProjectType>, IProjectTypeRepository
    {
        public ProjectTypeRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}