using BusinessCard.BusinessLogicLayer.DTOs;
using BusinessCard.BusinessLogicLayer.DTOs.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISelfEmployedService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<List<ServiceDto>> GetAllServicesAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        public Task<string> GetFullDescriptionAsync(int serviceId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        public Task<List<ReviewDto>> GetFortyReviews(int serviceId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        public Task<AdvancedServiceDto> GetAdvancedService(int serviceId);
    }
}