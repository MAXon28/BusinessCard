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
        public IEnumerable<ProjectDto> Projects { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FiltersDtoOut Filters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GeneralInformationDto GeneralInformation { get; set; }
    }
}