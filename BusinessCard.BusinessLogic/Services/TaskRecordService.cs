using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Interfaces.Utils.TasksRecords;
using BusinessCard.BusinessLogicLayer.Utils.Attributes;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using BusinessCard.Entities.DTO.Service;
using DapperAssistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="ITaskRecordService"/>
    internal class TaskRecordService : ITaskRecordService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskRecordRepository _taskRecordRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskRecordDataFactory _taskRecordDataFactory;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskRecordTextFactory _taskRecordTextFactory;

        public TaskRecordService(ITaskRecordRepository taskRecordRepository, ITaskRecordDataFactory taskRecordDataFactory, ITaskRecordTextFactory taskRecordTextFactory)
        {
            _taskRecordRepository = taskRecordRepository;
            _taskRecordDataFactory = taskRecordDataFactory;
            _taskRecordTextFactory = taskRecordTextFactory;
        }
        

        /// <inheritdoc/>
        public TaskRecord CreateRecordByUser(RecordTypes recordType, int taskId = -1)
        {
            var recordText = _taskRecordTextFactory.GetTaskRecordText(recordType);
            return new()
            {
                TaskId = taskId,
                Text = recordText.GetText(),
                Date = DateTime.Now,
                RoleId = (int)RoleTypes.User,
                ReadByUser = true
            };
        }

        /// <inheritdoc/>
        public TaskRecord CreateRecordByMAXon28Team(int taskId, string role, RecordTypes recordType)
        {
            var recordText = _taskRecordTextFactory.GetTaskRecordText(recordType);
            return new()
            {
                TaskId = taskId,
                Text = recordText.GetText(),
                Date = DateTime.Now,
                RoleId = (int)role.ToEnum<RoleTypes>(),
                ReadByMAXon28Team = true
            };
        }

        /// <inheritdoc/>
        public TaskRecord CreateRecordByMAXon28Team<T>(int taskId, string role, RecordTypes recordType, T additionalData)
        {
            var (replacablePart, recordTemplate) = _taskRecordDataFactory.GetTaskRecordData(recordType).GetData();
            return new()
            {
                TaskId = taskId,
                Text = recordTemplate.Replace(replacablePart, $"{additionalData}"),
                Date = DateTime.Now,
                RoleId = (int)role.ToEnum<RoleTypes>(),
                ReadByMAXon28Team = true
            };
        }

        public async Task<IReadOnlyCollection<Record>> GetTaskRecordsAsync(int taskId)
        {
            var querySettings = new QuerySettings
            {
                ConditionField = "TaskId",
                ConditionFieldValue = taskId,
                ConditionType = ConditionType.EQUALLY
            };
            var taskRecords = await _taskRecordRepository.GetWithConditionAsync(querySettings);
            return GetRecords(taskRecords).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskRecords">  </param>
        /// <returns>  </returns>
        private static IEnumerable<Record> GetRecords(IEnumerable<TaskRecord> taskRecords)
            => taskRecords
            .OrderBy(x => x.Date)
            .Select(x => new Record
            {
                Text = x.Text,
                CreationDate = x.Date.ConvertToReadableFormatWithTime(),
                Role = x.Role.Name,
                IsMAXonTeam = BelongsMAXonTeam(x.Role.Name.ToEnum<RoleTypes>())
            });

        /// <summary>
        /// Принадлежит ли пользователь к команде MAXon28 (опредление стороны, после действия которой последовала запись - либо со стороны пользователя, либо со стороны команды MAXon28)
        /// </summary>
        /// <param name="role"> Уровень прав доступа </param>
        /// <returns> Принадлежит ли пользователь к команде MAXon28 </returns>
        private static bool BelongsMAXonTeam(RoleTypes role)
        {
            var fieldInfo = role.GetType().GetField(role.ToString());
            return ((MAXonTeamAttribute[])fieldInfo.GetCustomAttributes(typeof(MAXonTeamAttribute), false)).Length > 0;
        }

        /// <inheritdoc/>
        public async Task MakeRecordsReadAsync(int taskId, RoleTypes role)
        {
            var makerOperation = GetNeededFunction(role);
            await makerOperation.Invoke(taskId);
        }

        /// <summary>
        /// Получить нужную функцию по отметке записей как прочитанные
        /// </summary>
        /// <param name="role"> Уровень прав доступа (в зависимости от этого определяется нужная функция) </param>
        /// <returns> Функция по отметке записей как прочитанные </returns>
        /// <exception cref="NotImplementedException"> Возникает в случае, если для заданного уровня прав доступа не реализована функция </exception>
        private Func<int, Task> GetNeededFunction(RoleTypes role)
            => role switch
            {
                RoleTypes.User => _taskRecordRepository.MakeRecordsReadForUserAsync,

                RoleTypes.MAXon28Team => _taskRecordRepository.MakeRecordsReadForMAXon28TeamAsync,

                _ => throw new NotImplementedException()
            };

        /// <inheritdoc/>
        public async Task AddRecordAsync(TaskRecord taskRecord)
            => await _taskRecordRepository.AddAsync(taskRecord);
    }
}