namespace BusinessCard.Entities.DTO.Work
{
    /// <summary>
    /// Информация по вакансии
    /// </summary>
    public class VacancyInfo
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Компания
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Позиция
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Формат работы
        /// </summary>
        public string WorkFormat { get; set; }

        /// <summary>
        /// Контакты
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// Зарплата
        /// </summary>
        public string Salary { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата создания вакансии
        /// </summary>
        public string CreationDate { get; set; }
    }
}