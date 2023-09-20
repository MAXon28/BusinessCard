using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Entities;
using BusinessCard.Entities.DTO.Service;
using BusinessCard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BusinessCard.Controllers
{
    public class MAXonTaskController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskService _taskService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IFileSaver _fileSaver;

        public MAXonTaskController(ITaskService taskService, IFileSaver fileSaver)
        {
            _taskService = taskService;
            _fileSaver = fileSaver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<string> AddTask(IFormCollection data)
        {
            const string technicalSpecificationFileDirectory = "TechnicalSpecifications";

            var fullTechnicalSpecificationFileName = data.Files.Count > 0 ? await _fileSaver.SaveFileAsync(data.Files[0], technicalSpecificationFileDirectory, true) : null;

            if (!data.TryGetValue("data", out var taskJson))
                throw new Exception("Не хватает данных");

            var newTask = JsonSerializer.Deserialize<NewTask>(taskJson);
            newTask.TechnicalSpecificationFileName = fullTechnicalSpecificationFileName;

            var (taskReceipt, taskUrl) = await _taskService.AddNewTaskAsync(newTask);

            return JsonConvert.SerializeObject(new
            {
                Receipt = taskReceipt,
                Url = taskUrl
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize]
        public async Task<string> GetUserTasksStatistic(int userId)
            => JsonConvert.SerializeObject(new
            {
                TasksStatistic = await _taskService.GetUserTasksStatisticAsync(userId)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
        public async Task<string> GetTasksStatistic()
            => JsonConvert.SerializeObject(new
            {
                TasksStatistic = await _taskService.GetAllTasksStatisticAsync()
            });

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult TaskHistory() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskFilters">  </param>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize]
        public async Task<string> GetUserTaskHistory([FromQuery] TaskFilters taskFilters)
            => JsonConvert.SerializeObject(new
            {
                TasksHistory = await _taskService.GetTasksHistoryAsync(taskFilters, Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value))
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskFilters">  </param>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
        public async Task<string> GetTaskHistory([FromQuery] TaskFilters taskFilters)
            => JsonConvert.SerializeObject(new
            {
                TasksHistory = await _taskService.GetTasksHistoryAsync(taskFilters)
            });

        [HttpGet]
        public IActionResult Task(string id) => View();

        [HttpGet]
        public async Task<string> GetTask(string taskUrl)
            => JsonConvert.SerializeObject(new
            {
                TaskInfo = User.IsInRole(Roles.MAXon28) || User.IsInRole(Roles.Admin)
                    ? await _taskService.GetTaskDetailForMAXon28TeamAsync(taskUrl)
                    : await _taskService.GetTaskDetailForUserAsync(taskUrl),
                IsAuthorizedUser = User.IsInRole(Roles.User)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId">  </param>
        /// <param name="taskUrl">  </param>
        /// <param name="statusId">  </param>
        /// <param name="status">  </param>
        /// <returns>  </returns>
        [HttpPost]
        [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
        public async Task<string> UpdateTaskStatus(int taskId, string taskUrl, int statusId, string status)
            => await _taskService.UpdateTaskStatusAsync(taskId, taskUrl, statusId, status, User.FindFirst(ClaimsIdentity.DefaultRoleClaimType).Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId">  </param>
        /// <param name="taskUrl">  </param>
        /// <param name="price">  </param>
        /// <returns>  </returns>
        [HttpPost]
        [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
        public async Task<string> AddTaskPrice(int taskId, string taskUrl, int price)
            => await _taskService.AddTaskPriceAsync(taskId, taskUrl, price, User.FindFirst(ClaimsIdentity.DefaultRoleClaimType).Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">  </param>
        /// <returns>  </returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
        public async Task<string> AddDoneTaskFile(IFormCollection data)
        {
            const string doneTaskFileDirectory = "DoneTasks";

            var fullDoneTaskFileName = data.Files.Count > 0 ? await _fileSaver.SaveFileAsync(data.Files[0], doneTaskFileDirectory) : null;

            if (!data.TryGetValue("taskId", out var taskId))
                throw new Exception("Не хватает данных");

            if (!data.TryGetValue("taskUrl", out var taskUrl))
                throw new Exception("Не хватает данных");

            var taskRecord = await _taskService.AddDoneTaskFileAsync(int.Parse(taskId.ToString()), taskUrl.ToString(), fullDoneTaskFileName, User.FindFirst(ClaimsIdentity.DefaultRoleClaimType).Value);

            return JsonConvert.SerializeObject(new
            {
                File = fullDoneTaskFileName,
                RecordText = taskRecord
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> NoticeDwonloadDoneTaskFile(int taskId, string taskUrl)
            => await _taskService.NoticeDownloadDoneTaskFileAsync(taskId, taskUrl);
    }
}