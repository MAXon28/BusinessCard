using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.DTOs.Store
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