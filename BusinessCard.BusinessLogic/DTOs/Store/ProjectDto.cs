using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.DTOs.Store
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? ClicksCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Compatibilities { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CodeUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreationDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ReviewsCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double AvgRating { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ClickTypeDto ClickType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ProjectImageDto> Images { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CodeLevelDto CodeLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RatingStatisticDto RatingStatistic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ProjectReviewDto Review { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> TechnicalRequirements { get; set; }
    }
}