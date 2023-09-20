using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// Контроллер счётчика задач (для команды MAXon28)
    /// </summary>
    [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
    public class TaskCounterController : Controller
    {
        /// <summary>
        /// Сервис счётчика задачи
        /// </summary>
        private readonly ITaskCounterService _taskCounterService;

        public TaskCounterController(ITaskCounterService taskCounterService) => _taskCounterService = taskCounterService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId">  </param>
        /// <param name="taskUrl">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<string> UpdateTaskCounter(int taskId, string taskUrl)
            => JsonConvert.SerializeObject(new
            {
                UpdateInfo = await _taskCounterService.UpdateTaskCounterAsync(taskId, taskUrl)
            });
    }
}