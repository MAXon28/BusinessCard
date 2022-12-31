namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore
{
    /// <summary>
    /// Тип сортировки
    /// </summary>
    internal enum SortTypes
    {
        /// <summary>
        /// По порядку (в порядке убывания)
        /// </summary>
        Standard = 1,

        /// <summary>
        /// По количеству отзывов
        /// </summary>
        CountReviews = 2,

        /// <summary>
        /// По количеству отзывов (в порядке убывания)
        /// </summary>
        CountReviewsDesc = 3,

        /// <summary>
        /// По среднему рейтингу
        /// </summary>
        Rating = 4,

        /// <summary>
        /// По среднему рейтингу (в порядке убывания)
        /// </summary>
        RatingDesc = 5
    }
}