using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.CacheProvider.Interfaces;
using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using BusinessCard.Entities.DTO.Review;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="ITaskReviewService"/>
    internal class TaskReviewService : ITaskReviewService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskReviewRepository _taskReviewRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskRecordService _taskRecordService;

        /// <summary>
        /// 
        /// </summary>
        private readonly ITaskCache _taskCache;

        public TaskReviewService(ITaskReviewRepository taskReviewRepository, ITaskRecordService taskRecordService, ITaskCache taskCache)
        {
            _taskReviewRepository = taskReviewRepository;
            _taskRecordService = taskRecordService;
            _taskCache = taskCache;
        }

        /// <inheritdoc/>
        public async Task<string> AddReviewAsync(int taskId, string stringTaskId, int userId, int rating, string text, int serviceId)
        {
            _taskCache.DeleteTaskDetail(stringTaskId);
            var record = _taskRecordService.CreateRecordByUser(RecordTypes.AddReview, taskId);
            var taskReview = new TaskReview
            {
                Rating = rating,
                Text = text,
                UserId = userId,
                TaskId = taskId,
                ServiceId = serviceId
            };
            await _taskReviewRepository.AddTaskReviewAsync(taskReview, record);
            return record.Text;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<ReviewData>> GetReviewsAsync(int serviceId)
        {
            throw new NotImplementedException();
        }
    }
}