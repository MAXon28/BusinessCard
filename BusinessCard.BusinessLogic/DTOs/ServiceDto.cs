using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.DTOs
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceDto
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
        public List<string> ShortDescriptions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Price { get; set; }
    }
}