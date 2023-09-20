using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Entities;
using BusinessCard.Entities.DTO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = $"{Roles.MAXon28}")]
    public class ServiceConditionsController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IConditionService _conditionService;

        public ServiceConditionsController(IConditionService conditionService) => _conditionService = conditionService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetServiceConditions(int serviceId) => Json(await _conditionService.GetConditionsByServiceAsync(serviceId));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<bool> AddCondition(ConditionDto condition) => await _conditionService.AddConditionAsync(condition);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<bool> UpdateCondition(ConditionDto condition) => await _conditionService.UpdateConditionAsync(condition);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionId">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<bool> DeleteCondition(int conditionId) => await _conditionService.DeleteConditionAsync(conditionId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetCalculatedValues() => Json(await _conditionService.GetCalculatedValuesForServiceAsync());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetServiceCalculatedValues(int serviceId) => Json(await _conditionService.GetServiceCalculatedValuesAsync(serviceId));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCalculatedValue"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> AddServiceCalculatedValue(ServiceCalculatedValueDto serviceCalculatedValue) => await _conditionService.AddServiceCalculatedValueAsync(serviceCalculatedValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCalculatedValue"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> UpdateServiceCalculatedValue(ServiceCalculatedValueDto serviceCalculatedValue) => await _conditionService.UpdateServiceCalculatedValueAsync(serviceCalculatedValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCalculatedValueId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> DeleteServiceCalculatedValue(int serviceCalculatedValueId) => await _conditionService.DeleteServiceCalculatedValueAsync(serviceCalculatedValueId);
    }
}