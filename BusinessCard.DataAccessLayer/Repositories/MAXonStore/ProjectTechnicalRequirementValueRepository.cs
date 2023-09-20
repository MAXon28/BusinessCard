using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore
{
    /// <inheritdoc cref="IProjectTechnicalRequirementValueRepository"/>
    internal class ProjectTechnicalRequirementValueRepository : StandardRepository<ProjectTechnicalRequirementValue>, IProjectTechnicalRequirementValueRepository
    {
        public ProjectTechnicalRequirementValueRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}