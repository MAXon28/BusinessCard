using System;

namespace BusinessCard.DataAccessLayer.Entities.Content
{
    /// <summary>
    /// Образование
    /// </summary>
    public class Education
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
        /// Дата начала учёбы
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания учёбы
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}