using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using Dapper;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonService
{
    /// <inheritdoc cref="ITaskRecordRepository"/>
    internal class TaskRecordRepository : StandardRepository<TaskRecord>, ITaskRecordRepository
    {
        public TaskRecordRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        /// <inheritdoc/>
        public async Task MakeRecordsReadForMAXon28TeamAsync(int taskId)
        {
            const string sqlQuery = @"update TaskRecords
                                      set ReadByMAXon28Team = 1
                                      where TaskId = @taskId and ReadByMAXon28Team = 0";
            using var connection = _dbConnectionKeeper.GetDbConnection();
            await connection.ExecuteAsync(sqlQuery, new { taskId });
        }

        /// <inheritdoc/>
        public async Task MakeRecordsReadForUserAsync(int taskId)
        {
            const string sqlQuery = @"update TaskRecords
                                      set ReadByUser = 1
                                      where TaskId = @taskId and ReadByUser = 0";
            using var connection = _dbConnectionKeeper.GetDbConnection();
            await connection.ExecuteAsync(sqlQuery, new { taskId });
        }
    }
}