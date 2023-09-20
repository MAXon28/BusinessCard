using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.Content
{
    /// <summary>
    /// Факт обо мне
    /// </summary>
    [SqlTable("FactsOnBusinessCard")]
    public class FactOnBusinessCard
    {
        /// <summary>
        /// Идентификатор факта
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Факт
        /// </summary>
        public string Data { get; set; }
    }
}