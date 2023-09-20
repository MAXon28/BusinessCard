using BusinessCard.BusinessLogicLayer.Interfaces.Utils.TasksRecords;

namespace BusinessCard.BusinessLogicLayer.Utils.TasksRecords.Data
{
    /// <inheritdoc cref="ITaskRecordData"/>
    internal abstract class TaskRecordData : ITaskRecordData
    {
        /// <summary>
        /// Заменяемая часть шаблона записи
        /// </summary>
        protected string _templateReplacablePart;

        /// <summary>
        /// Шаблон записи
        /// </summary>
        protected string _recordTemplate;

        /// <inheritdoc/>
        public (string TemplateReplacablePart, string RecordTemplate) GetData() => (_templateReplacablePart, _recordTemplate);
    }
}