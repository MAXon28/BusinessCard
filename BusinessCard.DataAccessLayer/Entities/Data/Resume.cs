namespace BusinessCard.DataAccessLayer.Entities.Data
{
    /// <summary>
    /// Данные по резюме
    /// </summary>
    public class Resume
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
        /// Информация по зарплатным ожиданиям
        /// </summary>
        public string Salary { get; set; }

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