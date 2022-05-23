using DapperAssistant.Annotations;
using System;

namespace BusinessCard.DataAccessLayer.Entities.Content
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("FactsOnBusinessCard")]
    public class FactOnBusinessCard
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Priority { get; set; }
    }
}