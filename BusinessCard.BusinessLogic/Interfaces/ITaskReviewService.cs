using BusinessCard.Entities.DTO.Review;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITaskReviewService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="stringTaskId"></param>
        /// <param name="userId"></param>
        /// <param name="rating"></param>
        /// <param name="text"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public Task<string> AddReviewAsync(int taskId, string stringTaskId, int userId, int rating, string text, int serviceId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public Task<IReadOnlyCollection<ReviewData>> GetReviewsAsync(int serviceId);
    }
}