using DapperAssistant.Annotations;
using System;
using System.Collections.Generic;

namespace BusinessCard.DataAccessLayer.Entities.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Conditions")]
    public class Condition
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConditionText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Services")]
        public int ServiceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public Service Service { get; set; }
    }
}