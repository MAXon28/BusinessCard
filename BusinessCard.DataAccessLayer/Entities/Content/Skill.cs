using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.Content
{
    /// <summary>
    /// Навык
    /// </summary>
    [SqlTable("Skills")]
    public class Skill
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
        /// Процент знаний
        /// </summary>
        public int PercentOfKnowledge { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}