namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// Тариф сервиса
    /// </summary>
    public class RateDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}