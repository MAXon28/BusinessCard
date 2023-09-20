namespace BusinessCard.DataAccessLayer.Entities.Content
{
    /// <summary>
    /// Резюме
    /// </summary>
    public class Work
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Позиция
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Информация по зарплате
        /// </summary>
        public string SalaryInfo { get; set; }

        /// <summary>
        /// Режим работы
        /// </summary>
        public string Schedule { get; set; }

        /// <summary>
        /// Технологический стек
        /// </summary>
        public string TechnologyStack { get; set; }
    }
}