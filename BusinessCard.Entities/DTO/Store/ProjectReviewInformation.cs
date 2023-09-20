using BusinessCard.Entities.DTO.Review;

namespace BusinessCard.Entities.DTO.Store
{
    public class ProjectReviewInformation
    {
        /// <summary>
        /// 
        /// </summary>
        public int ReviewsCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ReviewsPagesCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double AvgRating { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RatingStatistic RatingStatistic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ReviewData Review { get; set; }
    }
}