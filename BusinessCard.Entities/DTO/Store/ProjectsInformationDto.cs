namespace BusinessCard.Entities.DTO.Store
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectsInformationDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int PagesCountByCurrentFilters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ProjectInformation> Projects { get; set; }
    }
}