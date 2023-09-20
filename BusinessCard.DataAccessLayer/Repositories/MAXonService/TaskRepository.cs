using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TaskStatus = BusinessCard.DataAccessLayer.Entities.MAXonService.TaskStatus;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonService
{
    /// <inheritdoc cref="ITaskRepository"/>
    internal class TaskRepository : StandardRepository<TaskCard>, ITaskRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskPersonalInfoRepository _taskPersonalInfoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskCounterRepository _taskCounterRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskRecordRepository _taskRecordRepository;

        public TaskRepository(
            DbConnectionKeeper dbConnectionKeeper, 
            ITaskPersonalInfoRepository taskPersonalInfoRepository, 
            ITaskCounterRepository taskCounterRepository,
            ITaskRecordRepository taskRecordRepository) : base(dbConnectionKeeper)
        {
            _taskPersonalInfoRepository = taskPersonalInfoRepository;
            _taskCounterRepository = taskCounterRepository;
            _taskRecordRepository = taskRecordRepository;
        }

        /// <inheritdoc/>
        public async Task<string> AddTaskAsync(TaskCard taskCard, TaskPersonalInfo taskPersonalInfo, TaskCounter taskCounter, TaskRecord taskRecord)
        {
            const string sqlProcedureName = "dbo.GetNextTaskNumberDetails";

            var parameters = new DynamicParameters();
            parameters.Add("@ServiceId", taskCard.ServiceId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ServiceAbbreviation", SqlDbType.VarChar, size: 5, dbType: DbType.StringFixedLength, direction: ParameterDirection.Output);
            parameters.Add("@NextTaskNumber", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            dbConnection.Open();
            using var transaction = dbConnection.BeginTransaction();

            await dbConnection.ExecuteAsync(sqlProcedureName, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

            taskCard.TaskNumber = parameters.Get<string>("@ServiceAbbreviation").Replace(" ", "") + parameters.Get<int>("@NextTaskNumber");
            var taskId = await AddAsync<int>(taskCard, dbConnection, transaction);

            taskPersonalInfo.TaskId = taskId;
            await _taskPersonalInfoRepository.AddAsync(taskPersonalInfo, dbConnection, transaction);

            if (taskCounter is not null)
            {
                taskCounter.TaskId = taskId;
                await _taskCounterRepository.AddAsync(taskCounter, dbConnection, transaction);
            }

            taskRecord.TaskId = taskId;
            await _taskRecordRepository.AddAsync(taskRecord, dbConnection, transaction);

            transaction.Commit();

            return taskCard.TaskNumber;
        }

        /// <inheritdoc/>
        public async Task<(int processCount, int doneCount, int rejectedCount, int unreadCount)> GetUserTasksStatisticAsync(int userId)
        {
            const string sqlQuery = @"SELECT COUNT(t.Id),
		                                     'ProcessCount' AS Name
                                        FROM Tasks t
                                            INNER JOIN TasksPersonalInformation tps
                                            ON t.Id = tps.TaskId AND tps.UserId = @userId
	                                        INNER JOIN TaskStatuses ts
	                                        ON t.TaskStatusId = ts.Id
                                        WHERE ts.StatusCode = 1
                                        UNION
                                        SELECT COUNT(t.Id),
		                                     'DoneCount' AS Name
                                        FROM Tasks t
                                            INNER JOIN TasksPersonalInformation tps
                                            ON t.Id = tps.TaskId AND tps.UserId = @userId
	                                        INNER JOIN TaskStatuses ts
	                                        ON t.TaskStatusId = ts.Id
                                        WHERE ts.StatusCode = 2
                                        UNION
                                        SELECT COUNT(t.Id),
		                                     'RejectedCount' AS Name
                                        FROM Tasks t
                                            INNER JOIN TasksPersonalInformation tps
                                            ON t.Id = tps.TaskId AND tps.UserId = @userId
	                                        INNER JOIN TaskStatuses ts
	                                        ON t.TaskStatusId = ts.Id
                                        WHERE ts.StatusCode = 3
                                        UNION
                                        SELECT COUNT(tr.ReadByMAXon28Team),
                                            'UnreadCount' AS Name
                                        FROM Tasks t
                                            INNER JOIN TasksPersonalInformation tpi
                                            ON t.Id = tpi.TaskId AND tpi.UserId = @userId
                                            INNER JOIN TaskRecords tr
                                            ON t.Id = tr.TaskId
                                        WHERE tr.ReadByUser = 0";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            var statisticDictionary = (await dbConnection.QueryAsync<int, string, KeyValuePair<string, int>>(
                    sqlQuery,
                    (count, name) => new KeyValuePair<string, int>(name, count),
                    new { userId },
                    splitOn: "Name")).ToDictionary(x => x.Key, x => x.Value);

            return (statisticDictionary["ProcessCount"], statisticDictionary["DoneCount"], statisticDictionary["RejectedCount"], statisticDictionary["UnreadCount"]);
        }

        /// <inheritdoc/>
        public async Task<(int processCount, int doneCount, int rejectedCount, int unreadCount)> GetTasksStatisticAsync()
        {
            const string sqlQuery = @"SELECT COUNT(t.Id),
		                                     'ProcessCount' AS Name
                                        FROM Tasks t
	                                        INNER JOIN TaskStatuses ts
	                                        ON t.TaskStatusId = ts.Id
                                            INNER JOIN TasksPersonalInformation tps
                                            ON t.Id = tps.TaskId
                                        WHERE ts.StatusCode = 1
                                        UNION
                                        SELECT COUNT(t.Id),
		                                     'DoneCount' AS Name
                                        FROM Tasks t
	                                        INNER JOIN TaskStatuses ts
	                                        ON t.TaskStatusId = ts.Id
                                            INNER JOIN TasksPersonalInformation tps
                                            ON t.Id = tps.TaskId
                                        WHERE ts.StatusCode = 2
                                        UNION
                                        SELECT COUNT(t.Id),
		                                     'RejectedCount' AS Name
                                        FROM Tasks t
	                                        INNER JOIN TaskStatuses ts
	                                        ON t.TaskStatusId = ts.Id
                                            INNER JOIN TasksPersonalInformation tps
                                            ON t.Id = tps.TaskId
                                        WHERE ts.StatusCode = 3
                                        UNION
                                        SELECT COUNT(tr.ReadByMAXon28Team),
                                            'UnreadCount' AS Name
                                        FROM TaskRecords tr
                                        WHERE tr.ReadByMAXon28Team = 0";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            var statisticDictionary = (await dbConnection.QueryAsync<int, string, KeyValuePair<string, int>>(
                    sqlQuery,
                    (count, name) => new KeyValuePair<string, int>(name, count),
                    splitOn: "Name")).ToDictionary(x => x.Key, x => x.Value);

            return (statisticDictionary["ProcessCount"], statisticDictionary["DoneCount"], statisticDictionary["RejectedCount"], statisticDictionary["UnreadCount"]);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TaskCard>> GetTasksAsync(string sqlQuery, DynamicParameters parameters)
        {
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QueryAsync<TaskCard, string, string, TaskStatus, int, TaskCard>(
                    sqlQuery,
                    (taskCard, serviceName, rateName, taskStatus, unreadRecordsCount) =>
                    {
                        taskCard.Service = new Service { Name = serviceName };
                        taskCard.Rate = new Rate { Name = rateName };
                        taskCard.TaskStatus = taskStatus;
                        taskCard.UnreadRecordsCount = unreadRecordsCount;
                        return taskCard;
                    },
                    parameters,
                    splitOn: "Id, ServiceName, RateName, Status, UnreadRecordsCount");
        }

        /// <inheritdoc/>
        public async Task<int> GetTasksCountAsync(string sqlQuery, DynamicParameters parameters)
        {
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.QuerySingleAsync<int>(sqlQuery, parameters);
        }

        /// <inheritdoc/>
        public async Task<TaskCard> GetTaskByStringIdAsync(string taskStringId)
        {
            const string sqlQuery = @"SELECT task.Id,
                                             task.TaskNumber,
                                             task.SuggestedPrice,
                                             task.FixedPrice,
                                             task.Deadline,
                                             task.TechnicalSpecification,
                                             task.TechnicalSpecificationFileName,
                                             task.DoneTaskFileName,
                                             task.StatusUpdateDate,
                                             task.TaskCreationDate,
                                             task.ServiceId,
                                             service.Name AS ServiceName,
	                                         rate.Name AS RateName,
                                             taskStatus.Status,
	                                         taskStatus.StatusCode,
                                             taskPersonalInfo.Connection,
                                             taskPersonalInfo.UserSurname,
                                             taskPersonalInfo.UserName,
                                             taskPersonalInfo.UserMiddleName,
                                             taskPersonalInfo.UserEmail,
                                             taskPersonalInfo.UserPhoneNumber,
                                             [user].Surname,
                                             [user].Name,
                                             [user].MiddleName,
                                             [user].Email,
                                             [user].PhoneNumber
									FROM Tasks task
                                        INNER JOIN Services service
                                        ON task.ServiceId = service.Id
                                        LEFT JOIN Rates rate
                                        ON task.RateId = rate.Id
                                        INNER JOIN TaskStatuses taskStatus
                                        ON task.TaskStatusId = taskStatus.Id
                                        INNER JOIN TasksPersonalInformation taskPersonalInfo
                                        ON taskPersonalInfo.TaskId = task.Id
                                        LEFT JOIN Users [user]
                                        ON taskPersonalInfo.UserId = [user].Id
                                    WHERE task.Url = @taskStringId;";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return (await dbConnection.QueryAsync<TaskCard, string, string, TaskStatus, TaskPersonalInfo, User, TaskCard>(
                    sqlQuery,
                    (taskCard, serviceName, rateName, taskStatus, personalInfo, user) =>
                    {
                        taskCard.Service = new Service { Name = serviceName };
                        taskCard.Rate = new Rate { Name = rateName };
                        taskCard.TaskStatus = taskStatus;
                        personalInfo.User = user;
                        taskCard.TaskPersonalInfo = personalInfo;
                        return taskCard;
                    },
                    new { taskStringId },
                    splitOn: "Id, ServiceName, RateName, Status, Connection, Surname")).FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task UpdateTaskStatusAsync(int taskId, int statusId, TaskRecord record)
        {
            const string updateSqlQuery = @"UPDATE Tasks
                                            SET TaskStatusId = @statusId
                                            WHERE Id = @taskId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            dbConnection.Open();
            using var transaction = dbConnection.BeginTransaction();
            await _taskRecordRepository.AddAsync(record, dbConnection, transaction);
            await dbConnection.ExecuteAsync(updateSqlQuery, new { statusId, taskId }, transaction);
            transaction.Commit();
        }

        /// <inheritdoc/>
        public async Task SetTaskPriceAsync(int taskId, int price, TaskRecord record)
        {
            const string updateSqlQuery = @"UPDATE Tasks
                                            SET FixedPrice = @price
                                            WHERE Id = @taskId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            dbConnection.Open();
            using var transaction = dbConnection.BeginTransaction();
            await _taskRecordRepository.AddAsync(record, dbConnection, transaction);
            await dbConnection.ExecuteAsync(updateSqlQuery, new { price, taskId }, transaction);
            transaction.Commit();
        }

        /// <inheritdoc/>
        public async Task AddDoneTaskFileAsync(int taskId, string filePath, TaskRecord record)
        {
            const string updateSqlQuery = @"UPDATE Tasks
                                            SET DoneTaskFileName = @filePath
                                            WHERE Id = @taskId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            dbConnection.Open();
            using var transaction = dbConnection.BeginTransaction();
            await _taskRecordRepository.AddAsync(record, dbConnection, transaction);
            await dbConnection.ExecuteAsync(updateSqlQuery, new { filePath, taskId }, transaction);
            transaction.Commit();
        }
    }
}