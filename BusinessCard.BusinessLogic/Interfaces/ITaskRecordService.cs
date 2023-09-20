using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.Entities.DTO.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса записей по задачам
    /// </summary>
    public interface ITaskRecordService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordType"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public TaskRecord CreateRecordByUser(RecordTypes recordType, int taskId = -1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="role"></param>
        /// <param name="recordType"></param>
        /// <returns></returns>
        public TaskRecord CreateRecordByMAXon28Team(int taskId, string role, RecordTypes recordType);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="taskId"></param>
        /// <param name="role"></param>
        /// <param name="recordType"></param>
        /// <param name="additionalData"></param>
        /// <returns></returns>
        public TaskRecord CreateRecordByMAXon28Team<T>(int taskId, string role, RecordTypes recordType, T additionalData);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Task<IReadOnlyCollection<Record>> GetTaskRecordsAsync(int taskId);

        /// <summary>
        /// Сделать записи прочитанными
        /// </summary>
        /// <param name="taskId"> Идентификатор задачи </param>
        /// <param name="role"> Уровень прав доступа в системе (делаем прочитанными записи для пользователя или для команды MAXon28) </param>
        /// <returns></returns>
        public Task MakeRecordsReadAsync(int taskId, RoleTypes role);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskRecord"></param>
        /// <returns></returns>
        public Task AddRecordAsync(TaskRecord taskRecord);
    }
}