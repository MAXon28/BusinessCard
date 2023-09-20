using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Interfaces.Utils;
using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Exceptions;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonService;
using BusinessCard.CacheProvider.Interfaces;
using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using BusinessCard.Entities.DTO.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="ITaskService"/>
    internal class TaskService : ITaskService
    {
        /// <summary>
        /// 
        /// </summary>
        private const int DefaultTaskStatusId = 1;

        /// <summary>
        /// 
        /// </summary>
        private const int TasksCountInPackage = 7;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskRepository _taskRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceCalculatedValueRepository _serviceCalculatedValueRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskCounterService _taskCounterService;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskRecordService _taskRecordService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IValidator _validator;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPagination _pagination;

        /// <summary>
        /// 
        /// </summary>
        private readonly ISelectionQueryBuilderFactory _selectionQueryBuilderFactory;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskCache _taskCache;

        public TaskService(
            ITaskRepository taskRepository,
            IServiceCalculatedValueRepository serviceCalculatedValueRepository,
            ITaskCounterService taskCounterService,
            ITaskRecordService taskRecordService,
            IValidator validator,
            IPagination pagination,
            ISelectionQueryBuilderFactory selectionQueryBuilderFactory,
            ITaskCache taskCache)
        {
            _taskRepository = taskRepository;
            _serviceCalculatedValueRepository = serviceCalculatedValueRepository;
            _taskCounterService = taskCounterService;
            _taskRecordService = taskRecordService;
            _validator = validator;
            _pagination = pagination;
            _selectionQueryBuilderFactory = selectionQueryBuilderFactory;
            _taskCache = taskCache;
        }

        /// <inheritdoc/>
        public async Task<(string taskReceipt, string taskUrl)> AddNewTaskAsync(NewTask newTaskDto)
        {
            var taskCreationDate = DateTime.Now;
            var newTask = new TaskCard
            {
                SuggestedPrice = newTaskDto.SuggestedPrice,
                FixedPrice = null,
                Deadline = newTaskDto.Deadline,
                TechnicalSpecification = newTaskDto.TechnicalSpecification,
                TechnicalSpecificationFileName = newTaskDto.TechnicalSpecificationFileName,
                ServiceId = newTaskDto.ServiceId,
                RateId = newTaskDto.RateId,
                TaskStatusId = DefaultTaskStatusId,
                StatusUpdateDate = taskCreationDate,
                TaskCreationDate = taskCreationDate,
                Url = Guid.NewGuid().ToString("N")
            };
            var taskCounter = await GetServiceCounterDataAsync(newTaskDto.RateId);
            var personalInfo = GetPersonalInfo(newTaskDto);
            var taskRecord = _taskRecordService.CreateRecordByUser(RecordTypes.CreateTask);
            return (await _taskRepository.AddTaskAsync(newTask, personalInfo, taskCounter, taskRecord), newTask.Url);
        }

        private async Task<TaskCounter> GetServiceCounterDataAsync(int? rateId)
        {
            if (rateId is null)
                return null;

            var data = await _serviceCalculatedValueRepository.GetCalculatedValueDataByRateAsync((int)rateId);
            return new()
            {
                CalculatedValueId = data.CalculatedValueId,
                Counter = data.Number
            };
        }

        private TaskPersonalInfo GetPersonalInfo(NewTask newTaskDto)
            => newTaskDto.UserId is null
                ? new()
                {
                    UserSurname = newTaskDto.UserSurname,
                    UserName = newTaskDto.UserName,
                    UserMiddleName = newTaskDto.UserMiddleName,
                    UserPhoneNumber = _validator.ValidatePhoneNumber(newTaskDto.UserPhoneNumber)
                                    ? newTaskDto.UserPhoneNumber
                                    : throw new MAXonValidationException("Неправильный формат номера телефона.", ValidationTypes.PhoneNumber),
                    UserEmail = _validator.ValidateEmail(newTaskDto.UserEmail)
                            ? newTaskDto.UserEmail
                            : throw new MAXonValidationException("Неправильный формат email.", ValidationTypes.Email),
                    Connection = newTaskDto.Connection
                }
                : new()
                {
                    Connection = newTaskDto.Connection,
                    UserId = newTaskDto.UserId
                };

        /// <inheritdoc/>
        public async Task<TasksStatistic> GetUserTasksStatisticAsync(int userId)
        {
            var (processCount, doneCount, rejectedCount, unreadCount) = await _taskRepository.GetUserTasksStatisticAsync(userId);
            return new()
            {
                ProcessCount = processCount,
                DoneCount = doneCount,
                RejectedCount = rejectedCount,
                UnreadCount = unreadCount
            };
        }

        /// <inheritdoc/>
        public async Task<TasksStatistic> GetAllTasksStatisticAsync()
        {
            var (processCount, doneCount, rejectedCount, unreadCount) = await _taskRepository.GetTasksStatisticAsync();
            return new()
            {
                ProcessCount = processCount,
                DoneCount = doneCount,
                RejectedCount = rejectedCount,
                UnreadCount = unreadCount
            };
        }

        /// <inheritdoc/>
        public async Task<TasksInformation> GetTasksHistoryAsync(TaskFilters filters, int userId = -1)
        {
            var settings = GetTaskRequestSettings(filters, userId);
            var tasks = GetTasksAsync(settings);
            var tasksCount = filters.NeedPackagesCount
                ? await GetTasksCountAsync(settings)
                : default;
            var packagesCount = tasksCount == default
                ? default
                : _pagination.GetPagesCount(TasksCountInPackage, tasksCount);
            return new()
            {
                TasksCount = tasksCount,
                TasksPackageCount = packagesCount,
                Tasks = await tasks
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters">  </param>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        private static RequestSettings GetTaskRequestSettings(TaskFilters filters, int userId)
        {
            var statusTypes = filters.TypeOfStatus.ToEnum<TaskStatusTypes>();
            return new TaskRequestSettings(filters.LastTaskId, TasksCountInPackage, filters.SearchText, userId, statusTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings">  </param>
        /// <returns>  </returns>
        private async Task<IReadOnlyCollection<TaskDto>> GetTasksAsync(RequestSettings settings)
        {
            var queryData = _selectionQueryBuilderFactory.GetQueryBuilder(QueryBuilderTypes.Tasks).GetQueryData(settings);
            var tasks = await _taskRepository.GetTasksAsync(queryData.SqlQuery, queryData.Parameters);
            return GetProcessedTasks(tasks).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings">  </param>
        /// <returns>  </returns>
        private async Task<int> GetTasksCountAsync(RequestSettings settings)
        {
            var selectionQueryBuilder = _selectionQueryBuilderFactory.GetQueryBuilder(QueryBuilderTypes.Tasks);
            selectionQueryBuilder.TypeOfSelect = SelectTypes.Count;
            var queryData = selectionQueryBuilder.GetQueryData(settings);
            return await _taskRepository.GetTasksCountAsync(queryData.SqlQuery, queryData.Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tasks">  </param>
        /// <returns>  </returns>
        private static IEnumerable<TaskDto> GetProcessedTasks(IEnumerable<TaskCard> tasks)
            => tasks.Select(x => new TaskDto
            {
                Id = x.Id,
                Receipt = x.TaskNumber,
                Name = x.Rate.Name is null
                    ? GetTaskName(x.Service.Name, x.TaskNumber)
                    : GetTaskName(x.Service.Name, x.Rate.Name, x.TaskNumber),
                Price = x.FixedPrice is not null
                    ? (int)x.FixedPrice
                    : -1,
                UpdateDate = x.StatusUpdateDate.ConvertToReadableFormatWithTime(),
                CreationDate = x.TaskCreationDate.ConvertToReadableFormatWithTime(),
                StatusDetail = new TaskStatusDetail
                {
                    StatusText = x.TaskStatus.Status,
                    StatusType = x.TaskStatus.StatusCode.ToEnum<TaskStatusTypes>().ToStringAttribute()
                },
                Url = x.Url,
                UnreadRecordsCount = x.UnreadRecordsCount
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskNameDetails">  </param>
        /// <returns>  </returns>
        private static string GetTaskName(params string[] taskNameDetails)
            => string.Join(".", taskNameDetails);

        /// <inheritdoc/>
        public async Task<TaskDetail> GetTaskDetailForUserAsync(string taskStringId)
        {
            if (!TryGetTaskDetailFromCache(taskStringId, out var taskDetail))
                taskDetail = await GetTaskDetailAsync(taskStringId);
            await _taskRecordService.MakeRecordsReadAsync(taskDetail.Data.Id, RoleTypes.User);
            return taskDetail;
        }

        /// <inheritdoc/>
        public async Task<TaskDetail> GetTaskDetailForMAXon28TeamAsync(string taskStringId)
        {
            if (!TryGetTaskDetailFromCache(taskStringId, out var taskDetail))
                taskDetail = await GetTaskDetailAsync(taskStringId);
            await _taskRecordService.MakeRecordsReadAsync(taskDetail.Data.Id, RoleTypes.MAXon28Team);
            taskDetail.HaveCounter = await _taskCounterService.HaveTaskCounterAsync(taskDetail.Data.Id);
            return taskDetail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskStringId"></param>
        /// <param name="taskDetail"></param>
        /// <returns></returns>
        private bool TryGetTaskDetailFromCache(string taskStringId, out TaskDetail taskDetail)
        {
            taskDetail = _taskCache.GetTaskDetail(taskStringId);
            return taskDetail is not null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskStringId"></param>
        /// <returns></returns>
        private async Task<TaskDetail> GetTaskDetailAsync(string taskStringId)
        {
            var taskData = await _taskRepository.GetTaskByStringIdAsync(taskStringId);
            var (userSurname, userName, userMiddleName, userEmail, userPhoneNumber) = GetUserDetail(taskData.TaskPersonalInfo);
            var fullUserName = BuildFullUserName(userSurname, userName, userMiddleName);
            var taskStatusType = taskData.TaskStatus.StatusCode.ToEnum<TaskStatusTypes>();
            var taskDetail = new TaskDetail
            {
                Data = GetTaskData(taskData, fullUserName, taskStatusType),
                PersonalInfo = GetPersonalInfo(fullUserName, userEmail, userPhoneNumber, taskData.TaskPersonalInfo.Connection),
                Records = await _taskRecordService.GetTaskRecordsAsync(taskData.Id)
            };
            _taskCache.SaveTaskDetail(taskStringId, taskDetail);
            return taskDetail;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="task">  </param>
        /// <param name="fullUserName">  </param>
        /// <param name="taskStatus">  </param>
        /// <returns>  </returns>
        private static ExtendedTaskData GetTaskData(TaskCard task, string fullUserName, TaskStatusTypes taskStatus)
            => new()
            {
                Id = task.Id,

                Receipt = task.TaskNumber,

                Name = GetTaskName(task),

                Price = task.FixedPrice is not null ? (int)task.FixedPrice : -1,

                UpdateDate = task.StatusUpdateDate.ConvertToReadableFormatWithTime(),

                CreationDate = task.TaskCreationDate.ConvertToReadableFormatWithTime(),

                StatusDetail = new TaskStatusDetail
                {
                    Id = task.TaskStatusId,
                    StatusText = task.TaskStatus.Status,
                    StatusType = taskStatus.ToStringAttribute()
                },

                ServiceName = task.Service.Name,

                RateName = task.Rate.Name,

                SuggestedPrice = task.SuggestedPrice,

                Deadline = task.Deadline is not null
                    ? ((DateTime)task.Deadline).ConvertToReadableFormat()
                    : string.Empty,

                TechnicalSpecification = task.TechnicalSpecification,

                TechnicalSpecificationFilePath = task.TechnicalSpecificationFileName,

                TechnicalSpecificationFileNameForUser = string.IsNullOrEmpty(task.TechnicalSpecificationFileName) is false
                    ? GetTechnicalSpecificationFileName(fullUserName, Path.GetExtension(task.TechnicalSpecificationFileName))
                    : string.Empty,

                DoneTaskFilePath = task.DoneTaskFileName,

                DoneTaskFileNameForUser = string.IsNullOrEmpty(task.DoneTaskFileName) is false && taskStatus == TaskStatusTypes.Done
                    ? GetDoneTaskFileName(fullUserName)
                    : string.Empty
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullUserName">  </param>
        /// <param name="userEmail">  </param>
        /// <param name="userPhoneNumber">  </param>
        /// <param name="connection">  </param>
        /// <returns>  </returns>
        private static PersonalInfo GetPersonalInfo(string fullUserName, string userEmail, string userPhoneNumber, string connection)
            => new()
            {
                FullName = fullUserName,
                UserEmail = userEmail,
                UserPhoneNumber = userPhoneNumber,
                Connection = connection
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task">  </param>
        /// <returns>  </returns>
        private static string GetTaskName(TaskCard task)
            => string.IsNullOrEmpty(task.Rate.Name)
            ? GetTaskName(task.Service.Name, task.TaskNumber)
            : GetTaskName(task.Service.Name, task.Rate.Name, task.TaskNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personalInfo">  </param>
        /// <returns>  </returns>
        private static (string userSurname, string userName, string userMiddleName, string userEmail, string userPhoneNumber) GetUserDetail(TaskPersonalInfo personalInfo)
            => personalInfo.User is null
            ? (personalInfo.UserSurname, personalInfo.UserName, personalInfo.UserMiddleName, personalInfo.UserEmail, personalInfo.UserPhoneNumber)
            : (personalInfo.User.Surname, personalInfo.User.Name, personalInfo.User.MiddleName, personalInfo.User.Email, personalInfo.User.PhoneNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surname">  </param>
        /// <param name="name">  </param>
        /// <param name="middleName">  </param>
        /// <returns>  </returns>
        private static string BuildFullUserName(string surname, string name, string middleName)
            => string.IsNullOrEmpty(middleName) is false
                ? GetFullUserName(surname, name, middleName)
                : GetFullUserName(surname, name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userNameDetail">  </param>
        /// <returns>  </returns>
        private static string GetFullUserName(params string[] userNameDetail)
            => string.Join(" ", userNameDetail);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullUserName">  </param>
        /// <param name="fileExtension">  </param>
        /// <returns>  </returns>
        private static string GetTechnicalSpecificationFileName(string fullUserName, string fileExtension)
            => $"{fullUserName}. ТЗ{fileExtension}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullUserName">  </param>
        /// <returns>  </returns>
        private static string GetDoneTaskFileName(string fullUserName)
            => $"{fullUserName}. Выполненное задание";

        /// <inheritdoc/>
        public async Task<string> UpdateTaskStatusAsync(int taskId, string stringTaskId, int statusId, string statusText, string role)
        {
            _taskCache.DeleteTaskDetail(stringTaskId);
            var record = _taskRecordService.CreateRecordByMAXon28Team(taskId, role, RecordTypes.UpdateStatus, statusText);
            await _taskRepository.UpdateTaskStatusAsync(taskId, statusId, record);
            return record.Text;
        }

        /// <inheritdoc/>
        public async Task<string> AddTaskPriceAsync(int taskId, string stringTaskId, int price, string role)
        {
            _taskCache.DeleteTaskDetail(stringTaskId);
            var record = _taskRecordService.CreateRecordByMAXon28Team(taskId, role, RecordTypes.AddPrice, price);
            await _taskRepository.SetTaskPriceAsync(taskId, price, record);
            return record.Text;
        }

        /// <inheritdoc/>
        public async Task<string> AddDoneTaskFileAsync(int taskId, string stringTaskId, string filePath, string role)
        {
            _taskCache.DeleteTaskDetail(stringTaskId);
            var record = _taskRecordService.CreateRecordByMAXon28Team(taskId, role, RecordTypes.AddTaskDoneFile);
            await _taskRepository.AddDoneTaskFileAsync(taskId, filePath, record);
            return record.Text;
        }

        /// <inheritdoc/>
        public async Task<string> NoticeDownloadDoneTaskFileAsync(int taskId, string stringTaskId)
        {
            _taskCache.DeleteTaskDetail(stringTaskId);
            var record = _taskRecordService.CreateRecordByUser(RecordTypes.DownloadTaskDoneFile, taskId);
            await _taskRecordService.AddRecordAsync(record);
            return record.Text;
        }
    }
}