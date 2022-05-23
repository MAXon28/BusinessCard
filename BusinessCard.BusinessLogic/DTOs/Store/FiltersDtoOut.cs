using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.DTOs.Store
{
    /// <summary>
    /// 
    /// </summary>
    public class FiltersDtoOut
    {
        /// <summary>
        /// 
        /// </summary>
        public List<FilterDtoOut> ProjectTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<FilterDtoOut> ProjectCategories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, List<FilterDtoOut>> ProjectCompatibilities { get; set; }
    }
}