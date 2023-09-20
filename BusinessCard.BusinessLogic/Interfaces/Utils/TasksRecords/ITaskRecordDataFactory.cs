using BusinessCard.BusinessLogicLayer.Utils.Enums;

namespace BusinessCard.BusinessLogicLayer.Interfaces.Utils.TasksRecords
{
    /// <summary>
    /// Фабрика создания данных записи задачи
    /// </summary>
    public interface ITaskRecordDataFactory
    {
        /// <summary>
        /// Получить данные записи задачи
        /// </summary>
        /// <param name="recordType"> Тип записи задачи </param>
        /// <returns> Данные записи задачи </returns>
        public ITaskRecordData GetTaskRecordData(RecordTypes recordType);
    }
}