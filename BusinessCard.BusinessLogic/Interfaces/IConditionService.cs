using BusinessCard.Entities.DTO;
using BusinessCard.Entities.DTO.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Сервис тарифов услуг
    /// </summary>
    public interface IConditionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        public Task<IReadOnlyCollection<ConditionDto>> GetConditionsByServiceAsync(int serviceId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition">  </param>
        /// <returns>  </returns>
        public Task<bool> AddConditionAsync(ConditionDto condition);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition">  </param>
        /// <returns>  </returns>
        public Task<bool> UpdateConditionAsync(ConditionDto condition);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionId">  </param>
        /// <returns>  </returns>
        public Task<bool> DeleteConditionAsync(int conditionId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<IReadOnlyCollection<CalculatedValueDto>> GetCalculatedValuesForServiceAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public Task<IReadOnlyCollection<ServiceCalculatedValueDto>> GetServiceCalculatedValuesAsync(int serviceId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCalculatedValue"></param>
        /// <returns></returns>
        public Task<bool> AddServiceCalculatedValueAsync(ServiceCalculatedValueDto serviceCalculatedValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCalculatedValue"></param>
        /// <returns></returns>
        public Task<bool> UpdateServiceCalculatedValueAsync(ServiceCalculatedValueDto serviceCalculatedValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCalculatedValueId"></param>
        /// <returns></returns>
        public Task<bool> DeleteServiceCalculatedValueAsync(int serviceCalculatedValueId);
    }
}