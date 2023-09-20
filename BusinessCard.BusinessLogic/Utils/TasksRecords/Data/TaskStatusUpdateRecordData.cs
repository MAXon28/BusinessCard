namespace BusinessCard.BusinessLogicLayer.Utils.TasksRecords.Data
{
    /// <summary>
    /// Данные записи обновления статуса задачи
    /// </summary>
    internal class TaskStatusUpdateRecordData : TaskRecordData
    {
        public TaskStatusUpdateRecordData()
        {
            _templateReplacablePart = "*status*";
            _recordTemplate = $"Обновил статус задачи: <b>{_templateReplacablePart}</b>";
        }
    }
}