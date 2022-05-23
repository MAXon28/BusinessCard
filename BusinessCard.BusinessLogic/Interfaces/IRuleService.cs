using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRuleService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        public Task<string> GetServiceRuleAsync(int serviceId);
    }
}