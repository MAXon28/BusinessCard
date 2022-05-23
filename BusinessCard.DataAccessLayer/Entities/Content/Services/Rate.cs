using DapperAssistant.Annotations;
using System;
using System.Collections.Generic;

namespace BusinessCard.DataAccessLayer.Entities.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Rates")]
    public class Rate
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Services")]
        public int ServiceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSpecificPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public Service Service { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public List<ConditionValue> ConditionsValues { get; set; }
    }
}