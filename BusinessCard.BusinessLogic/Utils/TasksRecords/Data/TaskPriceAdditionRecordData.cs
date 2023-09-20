namespace BusinessCard.BusinessLogicLayer.Utils.TasksRecords.Data
{
    internal class TaskPriceAdditionRecordData : TaskRecordData
    {
        public TaskPriceAdditionRecordData()
        {
            _templateReplacablePart = "*price*";
            _recordTemplate = $"Установил цену за выполнение задачи: <b>{_templateReplacablePart} ₽</b>";
        }
    }
}