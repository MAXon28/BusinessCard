using BusinessCard.DataAccessLayer.Entities.Data;
using DapperAssistant.Annotations;
using System;

namespace BusinessCard.DataAccessLayer.Entities.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("TaskRecords")]
    [NeedInsertId]
    public class TaskRecord
    {
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Tasks")]
        public int TaskId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Roles")]
        public int RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ReadByUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ReadByMAXon28Team { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public Role Role { get; set; }
    }
}