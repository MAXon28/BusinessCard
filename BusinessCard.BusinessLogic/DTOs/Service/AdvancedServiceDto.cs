using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.DTOs.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class AdvancedServiceDto
    {
        public AdvancedServiceDto(int serviceId, string serviceName, string defaultPrice, bool needTechnicalSpecification, bool prePrice, bool preDeadline)
        {
            ServiceId = serviceId;
            ServiceName = serviceName;
            DefaultPrice = defaultPrice;
            NeedTechnicalSpecification = needTechnicalSpecification;
            PrePrice = prePrice;
            PreDeadline = preDeadline;
            Rates = new List<RateDto>();
        }

        /// <summary>
        /// 
        /// </summary>
        public int ServiceId { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ServiceName { get; }

        /// <summary>
        /// 
        /// </summary>
        public string DefaultPrice { get; }

        /// <summary>
        /// +
        /// </summary>
        public bool NeedTechnicalSpecification { get; set; }

        /// <summary>
        /// +
        /// </summary>
        public bool PrePrice { get; }

        /// <summary>
        /// +
        /// </summary>
        public bool PreDeadline { get; }

        /// <summary>
        /// 
        /// </summary>
        public List<RateDto> Rates { get; }
    }
}