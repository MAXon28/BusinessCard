namespace BusinessCard.Entities.DTO.Work
{
    /// <summary>
    /// Статистика по вакансиям
    /// </summary>
    public class VacanciesStatistic
    {
        /// <summary>
        /// Количество вакансий
        /// </summary>
        public int VacanciesCount { get; set; }

        /// <summary>
        /// Количество непросмотренных вакансий
        /// </summary>
        public int VacanciesNotViewedCount { get; set; }
    }
}