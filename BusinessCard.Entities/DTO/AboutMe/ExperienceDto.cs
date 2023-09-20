namespace BusinessCard.Entities.DTO.AboutMe
{
    /// <summary>
    /// Опыт работы
    /// </summary>
    public class ExperienceDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Компания
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Позиция
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата начала работы
        /// </summary>
        public string FromDate { get; set; }

        /// <summary>
        /// Дата окончания работы
        /// </summary>
        public string ToDate { get; set; }
    }
}