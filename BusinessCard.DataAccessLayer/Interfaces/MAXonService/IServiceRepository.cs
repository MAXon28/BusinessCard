using BusinessCard.DataAccessLayer.Entities.MAXonService;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonService
{
    /// <summary>
    /// Репозиторий сервисов
    /// </summary>
    public interface IServiceRepository : IRepository<Service>
    {
        /// <summary>
        /// Получить полное описание сервиса
        /// </summary>
        /// <param name="serviceId"> Идентификатор сервиса </param>
        /// <returns> Полное описание сервиса </returns>
        public Task<string> GetFullDescriptionByServiceId(int serviceId);

        /// <summary>
        /// Получить сервис
        /// </summary>
        /// <param name="serviceId"> Идентификатор сервиса </param>
        /// <returns> Сервис </returns>
        public Task<Service> GetService(int serviceId);
    }
}