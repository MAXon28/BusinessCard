using BusinessCard.Entities.DTO.Service;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    public interface ITaskService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newTaskDto">  </param>
        /// <returns>  </returns>
        public Task<(string taskReceipt, string taskUrl)> AddNewTaskAsync(NewTask newTaskDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<TasksStatistic> GetUserTasksStatisticAsync(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<TasksStatistic> GetAllTasksStatisticAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters">  </param>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<TasksInformation> GetTasksHistoryAsync(TaskFilters filters, int userId = -1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskStringId">  </param>
        /// <returns>  </returns>
        public Task<TaskDetail> GetTaskDetailForUserAsync(string taskStringId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskStringId">  </param>
        /// <returns>  </returns>
        public Task<TaskDetail> GetTaskDetailForMAXon28TeamAsync(string taskStringId);

        /// <summary>
        /// Обновить статус задачи
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <param name="stringTaskId"> Строковый идентификатор задачи </param>
        /// <param name="statusId"> Идентификатор статуса </param>
        /// <param name="statusText"> Текст статуса </param>
        /// <param name="role"> Права доступа пользователя, который обновляет статус </param>
        /// <returns> Текстовая запись с информацией об обновлении статуса задачи </returns>
        public Task<string> UpdateTaskStatusAsync(int taskId, string stringTaskId, int statusId, string statusText, string role);

        /// <summary>
        /// Установить цену за выполнение задачи
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <param name="stringTaskId"> Строковый идентификатор задачи </param>
        /// <param name="price"> Цена </param>
        /// <param name="role"> Права доступа пользователя, который установил цену </param>
        /// <returns> Текстовая запись с информацией об установлении цены за задачу </returns>
        public Task<string> AddTaskPriceAsync(int taskId, string stringTaskId, int price, string role);

        /// <summary>
        /// Добавить файл выполненной задачи
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <param name="stringTaskId"> Строковый идентификатор задачи </param>
        /// <param name="filePath"> Полное наименование файла </param>
        /// <param name="role"> Права доступа пользователя, который добавил файл с выполненным заданием </param>
        /// <returns> Текстовая запись с информацией о добавленном файле </returns>
        public Task<string> AddDoneTaskFileAsync(int taskId, string stringTaskId, string filePath, string role);

        /// <summary>
        /// Отметить информацию о загрузке пользователем файла выполненной задачи
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <param name="stringTaskId"> Строковый идентификатор задачи </param>
        /// <returns> Текстовая запись с информацией о скачивании файла </returns>
        public Task<string> NoticeDownloadDoneTaskFileAsync(int taskId, string stringTaskId);
    }
}