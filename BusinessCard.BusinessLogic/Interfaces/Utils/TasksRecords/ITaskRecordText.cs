namespace BusinessCard.BusinessLogicLayer.Interfaces.Utils.TasksRecords
{
    /// <summary>
    /// Текст записи задачи
    /// </summary>
    public interface ITaskRecordText
    {
        /// <summary>
        /// Получить текст записи задачи
        /// </summary>
        /// <returns> Текст записи задачи </returns>
        public string GetText();
    }
}