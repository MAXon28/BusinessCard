namespace BusinessCard.Entities.DTO.AboutMe
{
    /// <summary>
    /// Данные обо мне
    /// </summary>
    public class AboutMeInfo
    {
        /// <summary>
        /// Биография
        /// </summary>
        public string Biography { get; set; }

        /// <summary>
        /// Навыки
        /// </summary>
        public IReadOnlyCollection<SkillDto> Skills { get; set; }

        /// <summary>
        /// Опыт работы
        /// </summary>
        public IReadOnlyCollection<ExperienceDto> Experience { get; set; }

        /// <summary>
        /// Образования
        /// </summary>
        public IReadOnlyCollection<EducationDto> Education { get; set; }
    }
}