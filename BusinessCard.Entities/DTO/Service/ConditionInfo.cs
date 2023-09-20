namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// Информация по условию
    /// </summary>
    public class ConditionInfo
    {
        /// <summary>
        /// Идентификатор условия
        /// </summary>
        public int ConditionId { get; set; }

        /// <summary>
        /// Условие
        /// </summary>
        public string ConditionValue { get; set; }

        /// <summary>
        /// Идентификатор значения условия
        /// </summary>
        public int ConditionValueId { get; set; }

        /// <summary>
        /// Текстовое значение условия
        /// </summary>
        public string ConditionValueText { get; set; }

        /// <summary>
        /// Признак того, что условие выполняется в данном тарифе
        /// </summary>
        public bool IsAvailable { get; set; }
    }
}