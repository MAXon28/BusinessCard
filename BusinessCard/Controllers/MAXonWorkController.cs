using BusinessCard.BusinessLogicLayer.DTOs;
using BusinessCard.BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    public class MAXonWorkController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IWorkService _workService;

        public MAXonWorkController(IWorkService workService)
        {
            _workService = workService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult Work() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetResume() => Json(await _workService.GetResumeAsync());

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult JobPlacement() => View();

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
    }
}