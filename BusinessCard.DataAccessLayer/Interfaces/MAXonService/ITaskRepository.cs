using BusinessCard.DataAccessLayer.Entities.MAXonService;
using Dapper;
using DapperAssistant;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITaskRepository : IRepository<TaskCard>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskCard">  </param>
        /// <param name="taskPersonalInfo">  </param>
        /// <param name="taskCounter">  </param>
        /// <param name="taskRecord">  </param>
        /// <returns>  </returns>
        public Task<string> AddTaskAsync(TaskCard taskCard, TaskPersonalInfo taskPersonalInfo, TaskCounter taskCounter, TaskRecord taskRecord);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<(int processCount, int doneCount, int rejectedCount, int unreadCount)> GetUserTasksStatisticAsync(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<(int processCount, int doneCount, int rejectedCount, int unreadCount)> GetTasksStatisticAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery">  </param>
        /// <param name="parameters">  </param>
        /// <returns>  </returns>
        public Task<IEnumerable<TaskCard>> GetTasksAsync(string sqlQuery, DynamicParameters parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery">  </param>
        /// <param name="parameters">  </param>
        /// <returns>  </returns>
        public Task<int> GetTasksCountAsync(string sqlQuery, DynamicParameters parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskStringId">  </param>
        /// <returns>  </returns>
        public Task<TaskCard> GetTaskByStringIdAsync(string taskStringId);

        /// <summary>
        /// Обновить статус задачи
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <param name="statusId"> Идентификатор статуса </param>
        /// <param name="record"> Запись задачи об обновлении статуса </param>
        /// <returns></returns>
        public Task UpdateTaskStatusAsync(int taskId, int statusId, TaskRecord record);

        /// <summary>
        /// Установить цену за выполнение задачи
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <param name="price"> Цена </param>
        /// <param name="record"> Запись с информацией об установлении цены </param>
        /// <returns></returns>
        public Task SetTaskPriceAsync(int taskId, int price, TaskRecord record);

        /// <summary>
        /// Добавить файл выполненной задачи
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <param name="filePath"> Наименование файла </param>
        /// <param name="record"> Запись с информацией о добавлении файла </param>
        /// <returns></returns>
        public Task AddDoneTaskFileAsync(int taskId, string filePath, TaskRecord record);
    }
}