using BusinessCard.BusinessLogicLayer.Utils.Attributes;

namespace BusinessCard.BusinessLogicLayer.Utils.Enums
{
    /// <summary>
    /// Типы статусов задачи
    /// </summary>
    internal enum TaskStatusTypes
    {
        /// <summary>
        /// Задачи со всеми статусами
        /// </summary>
        [MAXon28("ALL")]
        All = 0,

        /// <summary>
        /// В процессе
        /// </summary>
        [MAXon28("PROCESS")]
        Process = 1,

        /// <summary>
        /// Выполнено
        /// </summary>
        [MAXon28("DONE")]
        Done = 2,

        /// <summary>
        /// Отклонено
        /// </summary>
        [MAXon28("REJECTED")]
        Rejected = 3
    }
}