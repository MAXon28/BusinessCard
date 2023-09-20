using BusinessCard.Entities.DTO.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Сервис статусов задачи
    /// </summary>
    public interface ITaskStatusService
    {
        /// <summary>
        /// Получить список статусов задачи
        /// </summary>
        /// <returns> Список статусов задачи </returns>
        public Task<IReadOnlyCollection<TaskStatusDetail>> GetStatusesAsync();
    }
}