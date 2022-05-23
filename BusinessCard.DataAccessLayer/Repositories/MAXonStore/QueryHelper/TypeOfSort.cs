namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore.QueryHelper
{
    /// <summary>
    /// Тип сортировки
    /// </summary>
    internal enum TypeOfSort
    {
        /// <summary>
        /// По порядку (в порядке убывания)
        /// </summary>
        Standard = 1,

        /// <summary>
        /// По количеству отзывов
        /// </summary>
        CountReviews,

        /// <summary>
        /// По количеству отзывов (в порядке убывания)
        /// </summary>
        CountReviewsDesc,

        /// <summary>
        /// По среднему рейтингу
        /// </summary>
        Rating,

        /// <summary>
        /// По среднему рейтингу (в порядке убывания)
        /// </summary>
        RatingDesc
    }
}