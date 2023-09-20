namespace BusinessCard.Entities.DTO.AboutMe
{
    /// <summary>
    /// Навык
    /// </summary>
    public class SkillDto
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