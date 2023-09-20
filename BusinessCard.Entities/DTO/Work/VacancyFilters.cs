namespace BusinessCard.Entities.DTO.Work
{
    /// <summary>
    /// Фильтры вакансий
    /// </summary>
    public class VacancyFilters
    {
        /// <summary>
        /// Идентификатор последней отображённой вакансии
        /// </summary>
        public int LastVacancyId { get; set; }

        /// <summary>
        /// Часть наименования вакансии (для поиска)
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// Необходимо подсчитать количество пакетов с вакансиями
        /// </summary>
        public bool NeedPackagesCount { get; set; }
    }
}