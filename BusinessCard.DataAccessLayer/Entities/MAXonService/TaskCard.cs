using DapperAssistant.Annotations;
using System;
using System.Collections.Generic;

namespace BusinessCard.DataAccessLayer.Entities.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Tasks")]
    public class TaskCard
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TaskNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SuggestedPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? FixedPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TechnicalSpecification { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TechnicalSpecificationFileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DoneTaskFileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Services")]
        public int ServiceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Rates")]
        public int? RateId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("TaskStatuses")]
        public int TaskStatusId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StatusUpdateDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime TaskCreationDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public Service Service { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public Rate Rate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public TaskStatus TaskStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public TaskPersonalInfo TaskPersonalInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public int UnreadRecordsCount { get; set; }
    }
}