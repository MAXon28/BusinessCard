using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using Dapper;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonService
{
    /// <inheritdoc cref="ITaskCounterRepository"/>
    internal class TaskCounterRepository : StandardRepository<TaskCounter>, ITaskCounterRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskRecordRepository _taskRecordRepository;

        public TaskCounterRepository(DbConnectionKeeper dbConnectionKeeper, ITaskRecordRepository taskRecordRepository) : base(dbConnectionKeeper) => _taskRecordRepository = taskRecordRepository;

        /// <inheritdoc/>
        public async Task<int> GetTaskCountersCountAsync(int taskId)
        {
            const string sqlQuery = "SELECT COUNT(TaskId) FROM TasksCounters WHERE TaskId = @taskId AND Counter != 0";
            using var connection = _dbConnectionKeeper.GetDbConnection();
            return await connection.QuerySingleAsync<int>(sqlQuery, new { taskId });
        }

        /// <inheritdoc/>
        public async Task<TaskCounter> GetTaskCounterAsync(int taskId)
        {
            const string sqlQuery = "SELECT Counter, CalculatedValueId FROM TasksCounters WHERE TaskId = @taskId";
            using var connection = _dbConnectionKeeper.GetDbConnection();
            return await connection.QuerySingleAsync<TaskCounter>(sqlQuery, new { taskId });
        }

        /// <inheritdoc/>
        public async Task UpdateTaskCounterAsync(int taskId, int counter, TaskRecord record)
        {
            const string updateSqlQuery = @"UPDATE TasksCounters
                                            SET Counter = @counter
                                            WHERE TaskId = @taskId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            dbConnection.Open();
            using var transaction = dbConnection.BeginTransaction();
            await _taskRecordRepository.AddAsync(record, dbConnection, transaction);
            await dbConnection.ExecuteAsync(updateSqlQuery, new { counter, taskId }, transaction);
            transaction.Commit();
        }
    }
}