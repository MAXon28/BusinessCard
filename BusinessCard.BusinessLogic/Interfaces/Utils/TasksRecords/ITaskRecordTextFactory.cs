using BusinessCard.BusinessLogicLayer.Utils.Enums;

namespace BusinessCard.BusinessLogicLayer.Interfaces.Utils.TasksRecords
{
    /// <summary>
    /// Фабрика создания текста записи задачи
    /// </summary>
    public interface ITaskRecordTextFactory
    {
        /// <summary>
        /// Получить текст записи задачи
        /// </summary>
        /// <param name="recordType"> Тип записи задачи </param>
        /// <returns> Текст записи задачи </returns>
        public ITaskRecordText GetTaskRecordText(RecordTypes recordType);
    }
}