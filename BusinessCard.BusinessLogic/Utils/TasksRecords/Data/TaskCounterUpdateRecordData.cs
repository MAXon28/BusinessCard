namespace BusinessCard.BusinessLogicLayer.Utils.TasksRecords.Data
{
    /// <summary>
    /// Данные записи обновления счётчика задачи
    /// </summary>
    internal class TaskCounterUpdateRecordData : TaskRecordData
    {
        public TaskCounterUpdateRecordData()
        {
            _templateReplacablePart = "*balance*";
            _recordTemplate = $"Осталось: <b>{_templateReplacablePart}</b>";
        }
    }
}