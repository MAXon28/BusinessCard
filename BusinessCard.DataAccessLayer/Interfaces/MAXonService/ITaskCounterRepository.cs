using BusinessCard.DataAccessLayer.Entities.MAXonService;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonService
{
    /// <summary>
    /// Репозиторий счётчика задачи
    /// </summary>
    public interface ITaskCounterRepository : IRepository<TaskCounter> 
    {
        /// <summary>
        /// Получить количество счётчиков задачи
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <returns> Количество счётчиков задачи </returns>
        public Task<int> GetTaskCountersCountAsync(int taskId);

        /// <summary>
        /// Получить счётчик задачи
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <returns> Счётчик задачи </returns>
        public Task<TaskCounter> GetTaskCounterAsync(int taskId);

        /// <summary>
        /// Обновить счётчик задачи
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <param name="counter"> Счётчик </param>
        /// <param name="record"> Запись с информацией об обновлении счётчика </param>
        /// <returns></returns>
        public Task UpdateTaskCounterAsync(int taskId, int counter, TaskRecord record);
    }
}