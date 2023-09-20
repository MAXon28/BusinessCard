using BusinessCard.Entities.DTO.AboutMe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Сервис информации обо мне
    /// </summary>
    public interface IAboutMeService
    {
        /// <summary>
        /// Получить информацию обо мне
        /// </summary>
        /// <returns> Информация обо мне </returns>
        public Task<AboutMeInfo> GetInformationAboutMe();

        /// <summary>
        /// Получить текст биографии
        /// </summary>
        /// <returns> Биография </returns>
        public Task<string> GetBiographyAsync();

        /// <summary>
        /// Получить все навыки
        /// </summary>
        /// <returns> Навыки </returns>
        public Task<IReadOnlyCollection<SkillDto>> GetSkillsAsync();

        /// <summary>
        /// Получить данные по опыту работы
        /// </summary>
        /// <returns> Данные по опыту работы </returns>
        public Task<IReadOnlyCollection<ExperienceDto>> GetExperienceAsync();

        /// <summary>
        /// Получить данные по образованию
        /// </summary>
        /// <returns> Данные по образованию </returns>
        public Task<IReadOnlyCollection<EducationDto>> GetEducationAsync();

        /// <summary>
        /// Обновить биографию
        /// </summary>
        /// <param name="biographyText"> Биография </param>
        /// <returns> Удалось обновить биографию </returns>
        public Task<bool> UpdateBiographyAsync(string biographyText);

        /// <summary>
        /// Добавить навык
        /// </summary>
        /// <param name="skill"> Навык </param>
        /// <returns> Удалось добавить навык </returns>
        public Task<bool> AddSkillAsync(SkillDto skill);

        /// <summary>
        /// Обновить навык
        /// </summary>
        /// <param name="skill"> Навык </param>
        /// <returns> Удалось обновить навык </returns>
        public Task<bool> UpdateSkillAsync(SkillDto skill);

        /// <summary>
        /// Удалить навык
        /// </summary>
        /// <param name="skillId"> Идентификатор навыка </param>
        /// <returns> Удалось удалить навык </returns>
        public Task<bool> DeleteSkillAsync(int skillId);

        /// <summary>
        /// Добавить данные по опыту работы
        /// </summary>
        /// <param name="experience"> Данные по опыту работы </param>
        /// <returns> Удалось добавить опыт работы </returns>
        public Task<bool> AddExperienceAsync(ExperienceDto experience);

        /// <summary>
        /// Обновить данные по опыту работы
        /// </summary>
        /// <param name="experience"> Данные по опыту работы </param>
        /// <returns> Удалось обновить опыт работы </returns>
        public Task<bool> UpdateExperienceAsync(ExperienceDto experience);

        /// <summary>
        /// Удалить данные по опыту работы
        /// </summary>
        /// <param name="experienceId"> Идентификатор опыта работы </param>
        /// <returns> Удалось удалить опыт работы </returns>
        public Task<bool> DeleteExperienceAsync(int experienceId);

        /// <summary>
        /// Добавить данные по образованию
        /// </summary>
        /// <param name="education"> Данные по образованию </param>
        /// <returns> Удалось добавить образование </returns>
        public Task<bool> AddEducationAsync(EducationDto education);

        /// <summary>
        /// Обновить данные по образованию
        /// </summary>
        /// <param name="education"> Данные по образованию </param>
        /// <returns> Удалось обновить образование </returns>
        public Task<bool> UpdateEducationAsync(EducationDto education);

        /// <summary>
        /// Удалить данные по образованию
        /// </summary>
        /// <param name="educationId"> Идентификатор образования </param>
        /// <returns> Удалось удалить образование </returns>
        public Task<bool> DeleteEducationAsync(int educationId);
    }
}