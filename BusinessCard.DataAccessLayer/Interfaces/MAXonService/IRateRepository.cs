using BusinessCard.DataAccessLayer.Entities.MAXonService;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonService
{
    /// <summary>
    /// Репозиторий тарифов сервисов
    /// </summary>
    public interface IRateRepository : IRepository<Rate> 
    {
        /// <summary>
        /// Получить тарифы сервиса
        /// </summary>
        /// <param name="serviceId"> Идентификатор сервиса </param>
        /// <returns> Тарифы сервиса </returns>
        public Task<IReadOnlyCollection<Rate>> GetRatesAsync(int serviceId);

        /// <summary>
        /// Получить данные по тарифу
        /// </summary>
        /// <param name="id"> Идентификатор тарифа </param>
        /// <returns> Данные по тарифу </returns>
        public Task<Rate> GetRateByIdAsync(int id);

        /// <summary>
        /// Обновить данные по тарифу
        /// </summary>
        /// <param name="rate"> Данные по тарифу </param>
        /// <returns> Количество обновлённых тарифов </returns>
        public Task<int> UpdateRateAsync(Rate rate);

        /// <summary>
        /// Обновить вычисляемое значение тарифа по услуге (сервису)
        /// </summary>
        /// <param name="id"> Идентификатор тарифа </param>
        /// <param name="serviceCounterId"> Идентификатор вычисляемого значения </param>
        /// <returns> Количество обновлённых тарифов </returns>
        public Task<int> UpdateRateCalculatedValueAsync(int id, int? serviceCounterId);
    }
}