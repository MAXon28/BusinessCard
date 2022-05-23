using DapperAssistant.Annotations;
using System;

namespace BusinessCard.DataAccessLayer.Entities.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("ConditionsValues")]
    public class ConditionValue
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Rates")]
        public int RateId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public Rate Rate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Conditions")]
        public Guid ConditionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public Condition Condition { get; set; }
    }
}