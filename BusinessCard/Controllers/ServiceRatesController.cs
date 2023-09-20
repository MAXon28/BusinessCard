using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Entities;
using BusinessCard.Entities.DTO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// Контроллер тарифов услуг
    /// </summary>
    [Authorize(Roles = $"{Roles.MAXon28}")]
    public class ServiceRatesController : Controller
    {
        /// <summary>
        /// Сервис тарифов услуги (сервиса)
        /// </summary>
        private readonly IRateService _rateService;

        public ServiceRatesController(IRateService rateService) => _rateService = rateService;

        /// <summary>
        /// Получить тарифы сервиса
        /// </summary>
        /// <param name="serviceId"> Идентификатор сервиса </param>
        /// <returns> Тарифы сервиса </returns>
        [HttpGet]
        public async Task<JsonResult> GetServiceRates(int serviceId) => Json(await _rateService.GetRatesAsync(serviceId));

        /// <summary>
        /// Получить тариф
        /// </summary>
        /// <param name="rateId"> Идентификатор тарифа </param>
        /// <returns> Тариф </returns>
        [HttpGet]
        public async Task<JsonResult> GetRate(int rateId) => Json(await _rateService.GetRateAsync(rateId));

        /// <summary>
        /// Добавить тариф
        /// </summary>
        /// <param name="rate"> Данные по тарифу </param>
        /// <returns> Успешно ли прошло добавление нового тарифа </returns>
        [HttpPost]
        public async Task<bool> AddRate(MAXonVersionRate rate) => await _rateService.AddRateAsync(rate);

        /// <summary>
        /// Обновить тариф
        /// </summary>
        /// <param name="rate"> Данные по тарифу </param>
        /// <param name="updateType"> Тип обновления </param>
        /// <returns> Успешно ли прошло обновление тарифа </returns>
        [HttpPost]
        public async Task<bool> UpdateRate(MAXonVersionRate rate, int updateType) => await _rateService.UpdateRateAsync(rate, updateType);

        /// <summary>
        /// Обновить вычисляемое значение тарифа
        /// </summary>
        /// <param name="rateId"> Идентификатор тарифа </param>
        /// <param name="serviceCounterId"> Идентификатор вычисляемого значения </param>
        /// <returns> Успешно ли прошло обновление вычисляемого значения тарифа </returns>
        [HttpPost]
        public async Task<bool> UpdateRateCalculatedValue(int rateId, int? serviceCounterId) => await _rateService.UpdateRateCalculatedValueAsync(rateId, serviceCounterId);
    }
}