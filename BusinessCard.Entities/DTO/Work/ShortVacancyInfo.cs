namespace BusinessCard.Entities.DTO.Work
{
    /// <summary>
    /// Короткая информация по вакансии
    /// </summary>
    public class ShortVacancyInfo
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
        /// Компания
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Зарплата
        /// </summary>
        public string Salary { get; set; }

        /// <summary>
        /// Дата создания публикации
        /// </summary>
        public string CreationDate { get; set; }

        /// <summary>
        /// Просмотрено командой MAXon28
        /// </summary>
        public bool ViewedByMAXon28Team { get; set; }
    }
}