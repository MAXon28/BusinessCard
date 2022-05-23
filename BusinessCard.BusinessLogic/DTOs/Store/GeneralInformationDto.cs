namespace BusinessCard.BusinessLogicLayer.DTOs.Store
{
    /// <summary>
    /// 
    /// </summary>
    public class GeneralInformationDto
    {
        /// <summary>
        /// 
        /// </summary>
        public CountInformationDto ProjectsCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CountInformationDto DownloadsCount { get; set; }

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