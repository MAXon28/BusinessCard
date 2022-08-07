using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.DTOs.Store
{
    /// <summary>
    /// 
    /// </summary>
    public class FiltersOut
    {
        /// <summary>
        /// 
        /// </summary>
        public List<FilterOut> ProjectTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<FilterOut> ProjectCategories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, List<FilterOut>> ProjectCompatibilities { get; set; }
    }
}