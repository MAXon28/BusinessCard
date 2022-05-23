using BusinessCard.DataAccessLayer.Entities.Content.Services;
using DapperAssistant.Annotations;
using System;

namespace BusinessCard.DataAccessLayer.Entities.Data
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Tasks")]
    [NeedInsertId]
    public class TaskCard
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TaskNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserSurname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserMiddleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserPhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SuggestedPrice { get; set; }

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
        [SqlForeignKey("Rates")]
        public Guid? RateId { get; set; }

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
        [RelatedSqlEntity]
        public Rate Rate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public TaskStatus TaskStatus { get; set; }
    }
}