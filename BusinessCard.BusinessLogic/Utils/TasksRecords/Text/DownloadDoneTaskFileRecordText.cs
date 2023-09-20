namespace BusinessCard.BusinessLogicLayer.Utils.TasksRecords.Text
{
    /// <summary>
    /// Текст записи загрузки файла выполненной задачи
    /// </summary>
    internal class DownloadDoneTaskFileRecordText : TaskRecordText
    {
        public DownloadDoneTaskFileRecordText() => _text = "Скачал файл с выполненной задачей.";
    }
}