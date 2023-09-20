namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// Информации по счётчику задачи
    /// </summary>
    public class TaskCounterInformation
    {
        /// <summary>
        /// Запись с информацией об обновлении счётчика задачи
        /// </summary>
        public string Record { get; set; }

        /// <summary>
        /// Признак финального подсчёта счётчика задачи
        /// </summary>
        public bool IsFinalCount { get; set; }
    }
}