using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    internal class ProjectCategoryRepository : StandardRepository<ProjectCategory>, IProjectCategoryRepository
    {
        public ProjectCategoryRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}