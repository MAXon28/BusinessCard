using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Entities;
using BusinessCard.Entities.DTO.AboutMe;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// Контроллер основных данных обо мне
    /// </summary>
    public class MAXonBusinessCardController : Controller
    {
        /// <summary>
        /// Сервис визитной карточки
        /// </summary>
        private readonly IBusinessCardService _businessCardService;

        /// <summary>
        /// Сервис "Обо мне"
        /// </summary>
        private readonly IAboutMeService _aboutMeService;

        public MAXonBusinessCardController(
            IBusinessCardService businessCardService, 
            IAboutMeService aboutMeService)
        {
            _businessCardService = businessCardService;
            _aboutMeService = aboutMeService;
        }

        /// <summary>
        /// Визитка
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Card() => View();

        /// <summary>
        /// Получить факты обо мне
        /// </summary>
        /// <returns> Факты обо мне </returns>
        [HttpGet]
        public async Task<JsonResult> GetMainFacts() => Json(await _businessCardService.GetFactsAsync());

        /// <summary>
        /// Обонвить факт
        /// </summary>
        /// <param name="fact"> Факт </param>
        /// <returns> Уадлось обновить факт </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> UpdateFact(Fact fact) => await _businessCardService.UpdateFactAsync(fact);

        /// <summary>
        /// Обо мне
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AboutMe() => View();

        /// <summary>
        /// Получить данные обо мне
        /// </summary>
        /// <returns> Данные обо мне </returns>
        [HttpGet]
        public async Task<JsonResult> GetAboutMeData() => Json(await _aboutMeService.GetInformationAboutMe());

        /// <summary>
        /// Получить биографию
        /// </summary>
        /// <returns> Биография </returns>
        [HttpGet]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<string> GetBiography() => await _aboutMeService.GetBiographyAsync();

        /// <summary>
        /// Обновить биографию
        /// </summary>
        /// <param name="biography"> Биография </param>
        /// <returns> Успешное обновление </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> UpdateBiography(string biography) => await _aboutMeService.UpdateBiographyAsync(biography);

        /// <summary>
        /// Получить навыки
        /// </summary>
        /// <returns> Навыки </returns>
        [HttpGet]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<JsonResult> GetSkills() => Json(await _aboutMeService.GetSkillsAsync());

        /// <summary>
        /// Добавить навык
        /// </summary>
        /// <param name="skill"> Навык </param>
        /// <returns> Успешное добавление </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> AddSkill(SkillDto skill) => await _aboutMeService.AddSkillAsync(skill);

        /// <summary>
        /// Обновить навык
        /// </summary>
        /// <param name="skill"> Навык </param>
        /// <returns> Успешное обновление </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> UpdateSkill(SkillDto skill) => await _aboutMeService.UpdateSkillAsync(skill);

        /// <summary>
        /// Удалить навык
        /// </summary>
        /// <param name="skillId"> Идентификатор навыка </param>
        /// <returns> Успешное удаление </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> DeleteSkill(int skillId) => await _aboutMeService.DeleteSkillAsync(skillId);

        /// <summary>
        /// Получить опыт работы
        /// </summary>
        /// <returns> Опыт работы </returns>
        [HttpGet]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<JsonResult> GetExperience() => Json(await _aboutMeService.GetExperienceAsync());

        /// <summary>
        /// Добавить опыт работы
        /// </summary>
        /// <param name="experience"> Опыт работы </param>
        /// <returns> Успешное добавление </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> AddExperience(ExperienceDto experience) => await _aboutMeService.AddExperienceAsync(experience);

        /// <summary>
        /// Обновить опыт работы
        /// </summary>
        /// <param name="experience"> Опыт работы </param>
        /// <returns> Успешное добавление </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> UpdateExperience(ExperienceDto experience) => await _aboutMeService.UpdateExperienceAsync(experience);

        /// <summary>
        /// Удалить опыт работы
        /// </summary>
        /// <param name="experienceId"> Идентификатор опыта работы </param>
        /// <returns> Успешное удаление </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> DeleteExperience(int experienceId) => await _aboutMeService.DeleteExperienceAsync(experienceId);

        /// <summary>
        /// Получить образование
        /// </summary>
        /// <returns> Образование </returns>
        [HttpGet]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<JsonResult> GetEducation() => Json(await _aboutMeService.GetEducationAsync());

        /// <summary>
        /// Добавить образование
        /// </summary>
        /// <param name="education"> Образование </param>
        /// <returns> Успешное добавление </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> AddEducation(EducationDto education) => await _aboutMeService.AddEducationAsync(education);

        /// <summary>
        /// Обновить образование
        /// </summary>
        /// <param name="education"> Образование </param>
        /// <returns> Успешное обновление </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> UpdateEducation(EducationDto education) => await _aboutMeService.UpdateEducationAsync(education);

        /// <summary>
        /// Удалить образование
        /// </summary>
        /// <param name="educationId"> Идентификатор образования </param>
        /// <returns> Успешное удаление </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> DeleteEducation(int educationId) => await _aboutMeService.DeleteEducationAsync(educationId);
    }
}