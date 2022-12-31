using BusinessCard.BusinessLogicLayer.DTOs;
using BusinessCard.BusinessLogicLayer.DTOs.Store;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Utils;
using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using DapperAssistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IProjectReviewService"/>
    public class ProjectReviewService : IProjectReviewService
    {
        /// <summary>
        /// Количество отзывов в одном пакете (для пагинации)
        /// </summary>
        private const int ReviewsCountInPackage = 28;

        /// <summary>
        /// Репозиторий отзывов
        /// </summary>
        private readonly IProjectReviewRepository _projectReviewRepository;

        /// <summary>
        /// Утилита для пагинации по отзывам
        /// </summary>
        private readonly PaginationtUtil _paginationUtil = new PaginationtUtil(ReviewsCountInPackage);

        public ProjectReviewService(IProjectReviewRepository projectReviewRepository) => _projectReviewRepository = projectReviewRepository;

        public async Task<List<Review>> GetReviewsAsync(int projectId, int reviewsPackageNumber)
        {
            var reviews = new List<Review>();

            foreach (var review in await _projectReviewRepository.GetReviewsAsync(projectId, _paginationUtil.GetOffset(reviewsPackageNumber)))
                reviews.Add(new Review
                {
                    UserName = review.UserName,
                    Rating = review.Rating,
                    Text = review.Text,
                    Date = review.Date.ToString("M")
                });

            return reviews;
        }

        public async Task<ProjectReviewInformation> GetReviewInformationAsync(int projectId, bool needLoadReview)
        {
            var statistic = await _projectReviewRepository.GetReviewStatisticAsync(projectId);

            var reviewInformation = new ProjectReviewInformation();
            reviewInformation.ReviewsCount = statistic.Sum(statistic => statistic.Value);
            reviewInformation.ReviewsPagesCount = _paginationUtil.GetPagesCount(reviewInformation.ReviewsCount);
            reviewInformation.AvgRating = Math.Round(statistic.Sum(rating => rating.Key * rating.Value) / (double)reviewInformation.ReviewsCount, 1);
            reviewInformation.RatingStatistic = new RatingStatistic
            {
                ExcellentCount = statistic.ContainsKey(5) ? (int)(Math.Round(statistic[5] / (double)reviewInformation.ReviewsCount, 2) * 100) : 0,
                GoodCount = statistic.ContainsKey(4) ? (int)(Math.Round(statistic[4] / (double)reviewInformation.ReviewsCount, 2) * 100) : 0,
                NotBadCount = statistic.ContainsKey(3) ? (int)(Math.Round(statistic[3] / (double)reviewInformation.ReviewsCount, 2) * 100) : 0,
                BadCount = statistic.ContainsKey(2) ? (int)(Math.Round(statistic[2] / (double)reviewInformation.ReviewsCount, 2) * 100) : 0,
                TerriblyCount = statistic.ContainsKey(0) ? (int)(Math.Round(statistic[1] / (double)reviewInformation.ReviewsCount, 2) * 100) : 0
            };
            if (reviewInformation.ReviewsCount > 0 && needLoadReview)
                reviewInformation.Review = await GetReviewAsync(projectId);

            return reviewInformation;
        }

        private async Task<Review> GetReviewAsync(int projectId)
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

            return new Review
            {
                UserName = review.UserName,
                Rating = review.Rating,
                Text = review.Text,
                Date = review.Date.ToString("M")
            };
        }

        public async Task<ProjectReviewInformation> CreateReview(int projectId, string userName, int rating, string reviewText)
        {
            if (rating < 1 || rating > 5)
                throw new Exception("Ошибка отправки отзыва. Рейтинг может быть от 1 до 5.");

            var newReview = new ProjectReview
            {
                UserName = userName,
                Rating = rating,
                Text = reviewText,
                Date = DateTime.Now,
                ProjectId = projectId,
                UserId = null
            };

            await _projectReviewRepository.AddAsync(newReview);

            return await GetReviewInformationAsync(projectId, false);
        }
    }
}