using BusinessCard.DataAccessLayer.Entities.MAXonService;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonService
{
    /// <summary>
    /// Репозиторий записей задач
    /// </summary>
    public interface ITaskRecordRepository : IRepository<TaskRecord> 
    {
        /// <summary>
        /// Сделать прочитанными записи задачи для пользователя
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <returns></returns>
        public Task MakeRecordsReadForUserAsync(int taskId);

        /// <summary>
        /// Сделать прочитанными записи задачи для команды MAXon28
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <returns></returns>
        public Task MakeRecordsReadForMAXon28TeamAsync(int taskId);
    }
}