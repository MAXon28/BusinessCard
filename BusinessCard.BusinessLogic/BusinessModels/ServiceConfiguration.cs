using System;

namespace BusinessCard.BusinessLogicLayer.BusinessModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ServiceConfiguration
    {
        public ServiceConfiguration() { }

        /// <summary>
        /// 
        /// </summary>
        public string ConcretePriceTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NotConcretePriceTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PriceTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrencyTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Currency { get; set; }
    }
}