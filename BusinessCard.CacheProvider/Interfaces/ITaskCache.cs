using BusinessCard.Entities.DTO.Service;

namespace BusinessCard.CacheProvider.Interfaces
{
    /// <summary>
    /// Кэш задачи
    /// </summary>
    public interface ITaskCache
    {
        /// <summary>
        /// Сохранить в кэш данные по задаче
        /// </summary>
        /// <param name="taskStringId"> Строковый идентификатор задачи </param>
        /// <param name="taskDetail"> Данные по задаче </param>
        public void SaveTaskDetail(string taskStringId, TaskDetail taskDetail);

        /// <summary>
        /// Получить из кэша данные по задаче
        /// </summary>
        /// <param name="taskStringId"> Строковый идентификатор задачи </param>
        /// <returns> Данные по задаче /returns>
        public TaskDetail? GetTaskDetail(string taskStringId);

        /// <summary>
        /// Удалить из кэша данные по задаче
        /// </summary>
        /// <param name="taskStringId"> Строковый идентификатор задачи </param>
        public void DeleteTaskDetail(string taskStringId);
    }
}