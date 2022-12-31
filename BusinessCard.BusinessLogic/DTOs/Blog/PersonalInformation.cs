using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.DTOs.Blog
{
    /// <summary>
    /// 
    /// </summary>
    public class PersonalInformation
    {
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, bool> SubscriptionsDictionary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, Dictionary<string, bool>> StatisticsByPost { get; set; }
    }
}