namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// Тариф сервиса (версия для MAXon8)
    /// </summary>
    public class MAXonVersionRate : RateDto
    {
        /// <summary>
        /// Цена
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Фиксированная цена или нет
        /// </summary>
        public bool IsSpecificPrice { get; set; }

        /// <summary>
        /// Опубликованный тариф или нет
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Идентификатор сервиса
        /// </summary>
        public int ServiceId { get; set; }

        /// <summary>
        /// Идентификатор счётчика условия по тарифу
        /// </summary>
        public int? ServiceCounterId { get; set; }

        /// <summary>
        /// Условия тарифа
        /// </summary>
        public IReadOnlyCollection<ConditionInfo> Conditions { get; set; }
    }
}