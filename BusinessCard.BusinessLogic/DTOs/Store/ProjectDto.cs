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
        public List<string> Compatibilities { get; set; }

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
    }
}