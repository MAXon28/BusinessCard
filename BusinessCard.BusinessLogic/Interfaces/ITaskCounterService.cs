using BusinessCard.Entities.DTO.Service;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Сервис счётчика задачи
    /// </summary>
    public interface ITaskCounterService
    {
        /// <summary>
        /// Есть ли счётчик у задачи
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <returns> Есть ли счётчик у задачи </returns>
        public Task<bool> HaveTaskCounterAsync(int taskId);

        /// <summary>
        /// Обновить счётчик задачи
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <param name="stringTaskId"> Строковый идентификатор задачи </param>
        /// <returns> Текстовая запись с информацией об обновлении счётчика </returns>
        public Task<TaskCounterInformation> UpdateTaskCounterAsync(int taskId, string stringTaskId);
    }
}