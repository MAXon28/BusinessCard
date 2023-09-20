namespace BusinessCard.BusinessLogicLayer.Interfaces.Utils.TasksRecords
{
    /// <summary>
    /// Данные по записи задачи
    /// </summary>
    public interface ITaskRecordData
    {
        /// <summary>
        /// Получить данные по записи задачи
        /// </summary>
        /// <returns> Заменяемая часть шаблона записи <br/> Шаблон записи </returns>
        public (string TemplateReplacablePart, string RecordTemplate) GetData();
    }
}