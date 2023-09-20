using DapperAssistant;
using DapperAssistant.Annotations;
using System;
using System.Collections.Generic;

namespace BusinessCard.DataAccessLayer.Entities.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Services")]
    public class Service
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
        public string FullDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ConcretePrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool NeedTechnicalSpecification { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool PrePrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool PreDeadline { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPublic { get; set; }

#nullable enable
        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public List<ShortDescription>? ShortDescriptions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public Dictionary<int, Rate>? Rates { get; set; }
#nullable disable
    }
}