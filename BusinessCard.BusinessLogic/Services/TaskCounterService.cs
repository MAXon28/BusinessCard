using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Interfaces.Utils;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using BusinessCard.CacheProvider.Interfaces;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using BusinessCard.Entities;
using BusinessCard.Entities.DTO.Service;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="ITaskCounterService"/>
    internal class TaskCounterService : ITaskCounterService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskCounterRepository _taskCounterRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskRecordService _taskRecordService;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskCache _taskCache;

        /// <summary>
        /// 
        /// </summary>
        private readonly IWordEnding _wordEnding;

        public TaskCounterService(ITaskCounterRepository taskCounterRepository, ITaskRecordService taskRecordService, ITaskCache taskCache, IWordEnding wordEnding)
        {
            _taskCounterRepository = taskCounterRepository;
            _taskRecordService = taskRecordService;
            _taskCache = taskCache;
            _wordEnding = wordEnding;
        }

        /// <inheritdoc/>
        public async Task<bool> HaveTaskCounterAsync(int taskId)
            => await _taskCounterRepository.GetTaskCountersCountAsync(taskId) == 1;

        /// <inheritdoc/>
        public async Task<TaskCounterInformation> UpdateTaskCounterAsync(int taskId, string stringTaskId)
        {
            _taskCache.DeleteTaskDetail(stringTaskId);
            var taskCounter = await _taskCounterRepository.GetTaskCounterAsync(taskId);
            _wordEnding.Init(taskCounter.CalculatedValueId.ToEnum<WordsVariants>());
            var updatedTaskCounter = taskCounter.Counter - 1;
            var data = $"{updatedTaskCounter} {_wordEnding.GetWord(updatedTaskCounter).ToLowerInvariant()}";
            var record = _taskRecordService.CreateRecordByMAXon28Team(taskId, Roles.MAXon28Bot, RecordTypes.UpdateTaskCounter, data);
            await _taskCounterRepository.UpdateTaskCounterAsync(taskId, updatedTaskCounter, record);
            return new()
            {
                Record = record.Text,
                IsFinalCount = updatedTaskCounter == 0
            };
        }
    }
}