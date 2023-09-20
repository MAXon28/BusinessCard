using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    internal class TaskPersonalInfoRepository : StandardRepository<TaskPersonalInfo>, ITaskPersonalInfoRepository
    {
        public TaskPersonalInfoRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}