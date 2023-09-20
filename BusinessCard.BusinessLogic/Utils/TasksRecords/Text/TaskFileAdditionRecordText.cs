namespace BusinessCard.BusinessLogicLayer.Utils.TasksRecords.Text
{
    /// <summary>
    /// Текст записи добавления файла выполненной задачи
    /// </summary>
    internal class TaskFileAdditionRecordText : TaskRecordText
    {
        public TaskFileAdditionRecordText() => _text = "Добавил файл с выполненной задачей.";
    }
}