using BusinessCard.BusinessLogicLayer.DTOs.Store;
using System.Collections.Generic;

namespace BusinessCard.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectsPageViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ProjectInformation> Projects { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FiltersOut Filters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GeneralInformation GeneralInformation { get; set; }
    }
}