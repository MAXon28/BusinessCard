using System;

namespace BusinessCard.DataAccessLayer.Entities.Content
{
    /// <summary>
    /// 
    /// </summary>
    public class Experience
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}