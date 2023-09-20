namespace BusinessCard.Entities.DTO.Store
{
    /// <summary>
    /// 
    /// </summary>
    public class GeneralInformation
    {
        /// <summary>
        /// 
        /// </summary>
        public CountInformation ProjectsCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CountInformation DownloadsCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double AvgRating { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PagesCount { get; set; }
    }
}