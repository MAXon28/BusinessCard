using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonService
{
    /// <inheritdoc cref="ITaskReviewRepository"/>
    internal class TaskReviewRepository : StandardRepository<TaskReview>, ITaskReviewRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskRecordRepository _taskRecordRepository;

        public TaskReviewRepository(DbConnectionKeeper dbConnectionKeeper, ITaskRecordRepository taskRecordRepository) : base(dbConnectionKeeper) => _taskRecordRepository = taskRecordRepository;

        /// <inheritdoc/>
        public async Task AddTaskReviewAsync(TaskReview taskReview, TaskRecord record)
        {
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            dbConnection.Open();
            using var transaction = dbConnection.BeginTransaction();
            await _taskRecordRepository.AddAsync(record, dbConnection, transaction);
            await AddAsync(taskReview, dbConnection, transaction);
            transaction.Commit();
        }
    }
}