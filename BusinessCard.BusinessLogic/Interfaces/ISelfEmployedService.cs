using BusinessCard.Entities.DTO.Review;
using BusinessCard.Entities.DTO.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Сервис для работы с услугами
    /// </summary>
    public interface ISelfEmployedService
    {
        /// <summary>
        /// Получить все опубликованные услуги
        /// </summary>
        /// <returns> Все опубликованные услуги </returns>
        public Task<IReadOnlyCollection<ServiceInfo>> GetAllPublicServicesAsync();

        /// <summary>
        /// Получить полное описание услуги
        /// </summary>
        /// <param name="serviceId"> Идентификатор услуги </param>
        /// <returns> Полное описание услуги </returns>
        public Task<string> GetFullDescriptionAsync(int serviceId);

        /// <summary>
        /// Получить отзывы по услуге
        /// </summary>
        /// <param name="serviceId"> Идентификатор услуги </param>
        /// <returns> Отзывы по услуге </returns>
        public Task<IReadOnlyCollection<ReviewData>> GetReviewsAsync(int serviceId);

        /// <summary>
        /// Получить расширенные данные услуги
        /// </summary>
        /// <param name="serviceId"> Идентификатор услуги </param>
        /// <returns> Расширенные данные услуги </returns>
        public Task<AdvancedServiceDto> GetAdvancedServiceAsync(int serviceId);

        /// <summary>
        /// Добавить услугу
        /// </summary>
        /// <param name="serviceDetailInfo"> Данные по услуге </param>
        /// <returns> Удалось добавить услугу или нет </returns>
        public Task<bool> AddServiceAsync(ServiceDetailInfo serviceDetailInfo);

        /// <summary>
        /// Обновить услугу
        /// </summary>
        /// <param name="serviceDetailInfo"> Данные по услуге </param>
        /// <param name="updateType"> Тип обновления </param>
        /// <returns>Удалось обновить услугу или нет  </returns>
        public Task<bool> UpdateServiceAsync(ServiceDetailInfo serviceDetailInfo, int updateType);

        /// <summary>
        /// Получить все услуги
        /// </summary>
        /// <returns> Все услуги </returns>
        public Task<IReadOnlyCollection<ServiceDto>> GetAllServicesAsync();

        /// <summary>
        /// Получить детали услуги
        /// </summary>
        /// <param name="serviceId"> Идентификатор услуги </param>
        /// <returns> Детали услуги </returns>
        public Task<ServiceDetailInfo> GetServiceDetailInfoAsync(int serviceId);
    }
}