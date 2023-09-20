using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using BusinessCard.DataAccessLayer.Interfaces.MAXon28Team;
using BusinessCard.Entities.DTO.Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskStatus = BusinessCard.DataAccessLayer.Entities.MAXonService.TaskStatus;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="ITaskStatusService"/>
    public class TaskStatusService : ITaskStatusService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskStatusRepository _taskStatusRepository;

        public TaskStatusService(ITaskStatusRepository taskStatusRepository)
        {
            _taskStatusRepository = taskStatusRepository;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TaskStatusDetail>> GetStatusesAsync()
            => GetTaskStatusesDetail(await _taskStatusRepository.GetAsync());

        /// <summary>
        /// Получить детальную информацию по статусам задач
        /// </summary>
        /// <param name="statuses"> Статусы задач </param>
        /// <returns> Детальная информация по статусам задач </returns>
        private static IReadOnlyCollection<TaskStatusDetail> GetTaskStatusesDetail(IEnumerable<TaskStatus> statuses)
            => statuses
            .Select(x => new TaskStatusDetail
            {
                Id = x.Id,
                StatusText = x.Status,
                StatusType = x.StatusCode.ToEnum<TaskStatusTypes>().ToStringAttribute()
            })
            .ToArray();
    }
}