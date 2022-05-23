using BusinessCard.BusinessLogicLayer.DTOs;
using BusinessCard.BusinessLogicLayer.DTOs.Results;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using System;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    public class TaskService : ITaskService
    {
        /// <summary>
        /// 
        /// </summary>
        private const int DefaultTaskStatusId = 1;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskCreationResult> AddNewTaskAsync(NewTaskDto newTaskDto)
        {
            var newTaskId = Guid.NewGuid();
            var taskCreationDate = DateTime.Now;
            var newTask = new TaskCard
            {
                Id = newTaskId,
                UserSurname = newTaskDto.UserSurname,
                UserName = newTaskDto.UserName,
                UserMiddleName = newTaskDto.UserMiddleName,
                UserPhoneNumber = newTaskDto.UserPhoneNumber,
                UserEmail = newTaskDto.UserEmail,
                Connection = newTaskDto.Connection,
                SuggestedPrice = newTaskDto.SuggestedPrice,
                Deadline = newTaskDto.Deadline,
                TechnicalSpecification = newTaskDto.TechnicalSpecification,
                TechnicalSpecificationFileName = newTaskDto.TechnicalSpecificationFileName,
                RateId = newTaskDto.RateId,
                TaskStatusId = DefaultTaskStatusId,
                StatusUpdateDate = taskCreationDate,
                TaskCreationDate = taskCreationDate
            };
            var newTaskNumber = await _taskRepository.AddTask(newTask, newTaskDto.ServiceId);

            return new TaskCreationResult
            {
                TaskAddress = newTaskId.ToString("N"),
                TaskNumber = newTaskNumber
            };
        }
    }
}