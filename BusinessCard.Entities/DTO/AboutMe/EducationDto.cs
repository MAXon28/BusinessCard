namespace BusinessCard.Entities.DTO.AboutMe
{
    /// <summary>
    /// Образование
    /// </summary>
    public class EducationDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Место учёбы
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата начала обучения
        /// </summary>
        public string FromDate { get; set; }

        /// <summary>
        /// Дата окончания обучения
        /// </summary>
        public string ToDate { get; set; }
    }
}