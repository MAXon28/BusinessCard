using System;
using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.DTOs.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class RateDto
    {
        public RateDto(Guid id, string rateName, string description, string ratePrice)
        {
            RateId = id;
            RateName = rateName;
            Description = description;
            RatePrice = ratePrice;
            Conditions = new Dictionary<string, ConditionValueDto>();
        }

        /// <summary>
        /// +
        /// </summary>
        public Guid RateId { get; set; }

        /// <summary>
        /// +
        /// </summary>
        public string RateName { get;}

        /// <summary>
        /// +
        /// </summary>
        public string Description { get;}

        /// <summary>
        /// +
        /// </summary>
        public string RatePrice { get;}

        /// <summary>
        /// +
        /// </summary>
        public Dictionary<string, ConditionValueDto> Conditions { get; }
    }
}