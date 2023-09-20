using BusinessCard.BusinessLogicLayer.Interfaces.Utils.TasksRecords;

namespace BusinessCard.BusinessLogicLayer.Utils.TasksRecords.Text
{
    /// <inheritdoc cref="ITaskRecordText"/>
    internal abstract class TaskRecordText : ITaskRecordText
    {
        /// <summary>
        /// Текст записи задачи
        /// </summary>
        protected string _text;

        public string GetText() => _text;
    }
}