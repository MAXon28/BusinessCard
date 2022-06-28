using BusinessCard.BusinessLogicLayer.DTOs.Store;
using BusinessCard.BusinessLogicLayer.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProjectReviewService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <returns>  </returns>
        public Task<ProjectReviewDto> GetReviewAsync(int projectId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  param>
        /// <param name="currentReviewId">  param>
        /// <param name="direction">  </param>
        /// <returns>  </returns>
        public Task<List<ProjectReviewDto>> GetReviewsAsync(int projectId, int currentReviewId, Direction direction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <returns>  </returns>
        public Task<Dictionary<int, int>> GetRatingStatisticAsync(int projectId);
    }
}