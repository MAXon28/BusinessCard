using BusinessCard.BusinessLogicLayer.DTOs;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MAXonBusinessCardController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IBusinessCardService _businessCardService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IAboutMeService _aboutMeService;

        /// <summary>
        /// 
        /// </summary>
        private readonly ISelfEmployedService _selfEmployedService;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskService _taskService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRuleService _ruleService;

        /// <summary>
        /// 
        /// </summary>
        private readonly FileSaver _fileSaver;

        public MAXonBusinessCardController(
            IBusinessCardService businessCardService, 
            IAboutMeService aboutMeService,
            ISelfEmployedService selfEmployedService,
            ITaskService taskService,
            IRuleService ruleService,
            FileSaver fileSaver)
        {
            _businessCardService = businessCardService;
            _aboutMeService = aboutMeService;
            _selfEmployedService = selfEmployedService;
            _taskService = taskService;
            _ruleService = ruleService;
            _fileSaver = fileSaver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult Card() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetMainFacts() => Json(await _businessCardService.GetFactsAsync());

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult AboutMe() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetAboutMeData() => Json(await _aboutMeService.GetInformationAboutMe());

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult Services() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetServices() => Json(await _selfEmployedService.GetAllServicesAsync());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetFullDescription(int id) => Json(await _selfEmployedService.GetFullDescriptionAsync(id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetReviews(int id)
        {
            return Json(await _selfEmployedService.GetFortyReviews(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult ServiceTaskRegistration(int serviceId)
        {
            ViewData["ServiceId"] = serviceId;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetServiceRates(int serviceId) => Json(await _selfEmployedService.GetAdvancedService(serviceId));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newTaskDto">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<JsonResult> AddTask(IFormCollection data)
        {
            const string technicalSpecificationFileDirectory = "TechnicalSpecifications";

            var fullTechnicalSpecificationFileName = data.Files.Count > 0 ? await _fileSaver.SaveFileAsync(data.Files[0], technicalSpecificationFileDirectory) : null;

            if (!data.TryGetValue("data", out var taskJson))
                throw new Exception();

            var newTask = JsonSerializer.Deserialize<NewTask>(taskJson);
            newTask.TechnicalSpecificationFileName = fullTechnicalSpecificationFileName;

            return Json(await _taskService.AddNewTaskAsync(newTask));
        }

        [HttpGet]
        public IActionResult Task(string id) => View();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<IActionResult> ServiceRule(int serviceId) => View();
    }
}