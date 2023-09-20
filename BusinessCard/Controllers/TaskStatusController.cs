using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// Контроллер статусов задачи (для команды MAXon28)
    /// </summary>
    [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
    public class TaskStatusController : Controller
    {
        /// <summary>
        /// Сервис статусов задачи
        /// </summary>
        private readonly ITaskStatusService _taskStatusService;

        public TaskStatusController(ITaskStatusService taskStatusService) => _taskStatusService = taskStatusService;

        /// <summary>
        /// Получить все возможные статусы задачи
        /// </summary>
        /// <returns> Статусы задачи </returns>
        [HttpGet]
        public async Task<string> GetTaskStatuses()
            => JsonConvert.SerializeObject(new
            {
                Statuses = await _taskStatusService.GetStatusesAsync()
            });
    }
}