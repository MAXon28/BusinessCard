using BusinessCard.DataAccessLayer.Entities.MAXonService;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonService
{
    /// <summary>
    /// Репозиторий краткой информации по сервису
    /// </summary>
    public interface IShortDescriptionRepository : IRepository<ShortDescription> 
    {
        /// <summary>
        /// Добавить краткую информацию по сервису
        /// </summary>
        /// <param name="shortDescriptions"> Краткая информация по сервису </param>
        /// <returns> Количество добавленных значений краткой информации </returns>
        public Task<int> AddShortDescriptionsAsync(IReadOnlyCollection<ShortDescription> shortDescriptions);

        /// <summary>
        /// Обновить краткую информацию по сервису
        /// </summary>
        /// <param name="shortDescriptions"> Краткая информация по сервису </param>
        /// <returns> Количество обновлённых значений краткой информации </returns>
        public Task<int> UpdateShortDescriptionsAsync(IReadOnlyCollection<ShortDescription> shortDescriptions);

        /// <summary>
        /// Получить краткую информацию по всем опубликованным сервисам
        /// </summary>
        /// <returns> Краткая информация по всем опубликованным сервисам </returns>
        public Task<IEnumerable<ShortDescription>> GetShortDescriptionsForPublicServicesAsync();
    }
}