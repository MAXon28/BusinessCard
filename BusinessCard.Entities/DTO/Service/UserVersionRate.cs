namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// Тариф сервиса (версия для пользователя)
    /// </summary>
    public class UserVersionRate : RateDto
    {
        /// <summary>
        /// Цена тарифа
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// Условия тарифа
        /// </summary>
        public IDictionary<string, ConditionValueDto> Conditions { get; set; }
    }
}