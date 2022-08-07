using DapperAssistant;
using DapperAssistant.Annotations;
using System;
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
        public int? ClicksCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProjectUrl { get; set; }

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
        public DateTime CreationDate { get; set; }

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
        [SqlForeignKey("ClickTypes", TypeOfJoin.LEFT)]
        public int ClickTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("CodeLevels")]
        public int CodeLevelId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public ProjectType ProjectType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public ProjectCategory ProjectCategory { get; set; }
#nullable enable
        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public ClickType? ClickType { get; set; }
#nullable disable
        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public CodeLevel CodeLevel { get; set; }

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