using DapperAssistant.Annotations;
using System.Collections.Generic;

namespace BusinessCard.DataAccessLayer.Entities.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Projects")]
    public class Project
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
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DownloadsCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("ProjectTypes")]
        public int TypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("ProjectCategories")]
        public int CategoryId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public ProjectType ProjectType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public ProjectCategory ProjectCategory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public int ReviewsCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public double Rating { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [NotSqlColumn]
        public List<string> Compatibilities { get; set; }
    }
}