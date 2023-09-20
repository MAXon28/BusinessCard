using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.Entities;
using BusinessCard.Entities.DTO.Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// Контроллер для работы
    /// </summary>
    public class MAXonWorkController : Controller
    {
        /// <summary>
        /// Сервис работы
        /// </summary>
        private readonly IWorkService _workService;

        public MAXonWorkController(IWorkService workService) => _workService = workService;

        /// <summary>
        /// Резюме
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Resume() => View();

        /// <summary>
        /// Получить резюме
        /// </summary>
        /// <returns> Резюме </returns>
        [HttpGet]
        public async Task<JsonResult> GetResume() => Json(await _workService.GetResumeAsync());

        /// <summary>
        /// Обновить флаг "Ищу работу"
        /// </summary>
        /// <param name="value"> Значение флага </param>
        /// <returns> Удалось ли обновить флаг </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> UpdateFlagForNeedWork(bool value) => await _workService.UpdateFlagAsync(value);

        /// <summary>
        /// Обновить резюме
        /// </summary>
        /// <param name="resume"> Резюме </param>
        /// <returns> Уадлось ли обновить резюме </returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> UpdateResume(Resume resume) => await _workService.UpdateResumeAsync(resume);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult VacancyCreator() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newVacancy">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<bool> CreateVacancy([FromBody] VacancyDto newVacancy)
        {
            return await _workService.TryAddVacancyAsync(newVacancy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
        public async Task<string> GetVacanciesStatistic()
            => JsonConvert.SerializeObject(new
            {
                VacanciesStatistic = await _workService.GetVacanciesStatisticAsync()
            });

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
        public IActionResult Vacancies() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
        public async Task<string> GetVacanciesInfo([FromQuery] VacancyFilters vacancyFilters)
        {
            var (shortVacanciesInfo, vacanciesCount, packagesCount) = await _workService.GetShortVacanciesDataAsync(vacancyFilters);
            return JsonConvert.SerializeObject(new
            {
                VacanciesInfo = shortVacanciesInfo,
                VacanciesCount = vacanciesCount,
                PackagesCount = packagesCount
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
        public IActionResult Vacancy(int id) => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
        public async Task<string> GetVacancy(int id)
            => JsonConvert.SerializeObject(new
            {
                Vacancy = await _workService.GetVacancyAsync(id)
            });
    }
}