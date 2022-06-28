using BusinessCard.BusinessLogicLayer.DTOs.Store;
using BusinessCard.BusinessLogicLayer.Enums;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using DapperAssistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IProjectReviewService"/>
    public class ProjectReviewService : IProjectReviewService
    {
        /// <summary>
        /// Репозиторий отзывов
        /// </summary>
        private readonly IProjectReviewRepository _projectReviewRepository;

        public ProjectReviewService(IProjectReviewRepository projectReviewRepository) => _projectReviewRepository = projectReviewRepository;

        public async Task<ProjectReviewDto> GetReviewAsync(int projectId)
        {
            var querySettings = new QuerySettings
            {
                ConditionField = "ProjectId",
                ConditionType = ConditionType.EQUALLY,
                ConditionFieldValue = projectId,
                CertainNumberOfRows = 1,
                NeedSortDescendingOrder = true
            };

            var review = (await _projectReviewRepository.GetWithConditionAsync(querySettings)).FirstOrDefault();

            return new ProjectReviewDto
            {
                UserName = review.UserName,
                Rating = review.Rating,
                Text = review.Text,
                Date = review.Date.ToString("M")
            };
        }

        public Task<List<ProjectReviewDto>> GetReviewsAsync(int projectId, int currentProjectId, Direction direction)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Dictionary<int, int>> GetRatingStatisticAsync(int projectId) => await _projectReviewRepository.GetReviewStatisticAsync(projectId);
    }
}