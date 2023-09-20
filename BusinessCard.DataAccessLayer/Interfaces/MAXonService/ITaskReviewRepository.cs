using BusinessCard.DataAccessLayer.Entities.MAXonService;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonService
{
    /// <summary>
    /// Репозиторий отзывов на задачи
    /// </summary>
    public interface ITaskReviewRepository : IRepository<TaskReview> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskReview"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public Task AddTaskReviewAsync(TaskReview taskReview, TaskRecord record);
    }
}