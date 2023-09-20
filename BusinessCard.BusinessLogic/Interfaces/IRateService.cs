using BusinessCard.Entities.DTO.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Сервис тарифов услуги (сервиса)
    /// </summary>
    public interface IRateService
    {
        /// <summary>
        /// Получить тарифы по услуге (сервису)
        /// </summary>
        /// <param name="serviceId"> Идентификатор услуги (сервиса) </param>
        /// <returns> Тарифы по услуге (сервису) </returns>
        public Task<IReadOnlyCollection<MAXonVersionRate>> GetRatesAsync(int serviceId);

        /// <summary>
        /// Получить данные по тарифу услуги (сервиса)
        /// </summary>
        /// <param name="rateId"> Идентификатор тарифа </param>
        /// <returns> Данные по тарифу услуги (сервиса) </returns>
        public Task<MAXonVersionRate> GetRateAsync(int rateId);

        /// <summary>
        /// Добавить тариф по услуге (сервису)
        /// </summary>
        /// <param name="rate"> Сформированные данные для нового тарифа </param>
        /// <returns> Удалось добавить тариф или нет </returns>
        public Task<bool> AddRateAsync(MAXonVersionRate rate);

        /// <summary>
        /// Обновить тариф по услуге (сервису)
        /// </summary>
        /// <param name="rate"> Данные тарифа </param>
        /// <param name="updateType"> Тип обновления </param>
        /// <returns> Уадлось обновить тариф или нет </returns>
        public Task<bool> UpdateRateAsync(MAXonVersionRate rate, int updateType);

        /// <summary>
        /// Обновить вычисляемое значение тарифа по услуге (сервису)
        /// </summary>
        /// <param name="rateId"> Идентификатор тарифа </param>
        /// <param name="serviceCounterId"> Идентификатор вычисляемого значения </param>
        /// <returns> Удалось обновить вычисляемое значение или нет </returns>
        public Task<bool> UpdateRateCalculatedValueAsync(int rateId, int? serviceCounterId);
    }
}