namespace BusinessCard.BusinessLogicLayer.Utils.Enums
{
    /// <summary>
    /// Типы записей задач
    /// </summary>
    public enum RecordTypes
    {
        /// <summary>
        /// Создание задачи
        /// </summary>
        CreateTask,

        /// <summary>
        /// Обновление статуса задачи
        /// </summary>
        UpdateStatus,

        /// <summary>
        /// Добавление цены за выполнение задачи
        /// </summary>
        AddPrice,

        /// <summary>
        /// Добавление файла с выполненной задачей
        /// </summary>
        AddTaskDoneFile,

        /// <summary>
        /// Обновление счётчика задачи
        /// </summary>
        UpdateTaskCounter,

        /// <summary>
        /// Скачивание файла с выполненной задачей
        /// </summary>
        DownloadTaskDoneFile,

        /// <summary>
        /// Отзыв по выполнению задачи
        /// </summary>
        AddReview
    }
}