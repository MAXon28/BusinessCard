using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXon28Team;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.MAXon28Team
{
    /// <inheritdoc cref="ITaskStatusRepository"/>
    internal class TaskStatusRepository : StandardRepository<TaskStatus>, ITaskStatusRepository
    {
        public TaskStatusRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}