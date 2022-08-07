using BusinessCard.BusinessLogicLayer.DTOs;
using BusinessCard.BusinessLogicLayer.DTOs.Store;
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
        /// <param name="projectId">  param>
        /// <param name="reviewsPackageNumber">  param>
        /// <returns>  </returns>
        public Task<List<Review>> GetReviewsAsync(int projectId, int reviewsPackageNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <param name="needLoadReview">  </param>
        /// <returns>  </returns>
        public Task<ProjectReviewInformation> GetReviewInformationAsync(int projectId, bool needLoadReview);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <param name="userName">  </param>
        /// <param name="rating">  </param>
        /// <param name="reviewText">  </param>
        /// <returns>  </returns>
        public Task<ProjectReviewInformation> CreateReview(int projectId, string userName, int rating, string reviewText);
    }
}