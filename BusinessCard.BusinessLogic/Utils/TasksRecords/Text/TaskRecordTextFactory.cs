using BusinessCard.BusinessLogicLayer.Interfaces.Utils.TasksRecords;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BusinessCard.BusinessLogicLayer.Utils.TasksRecords.Text
{
    /// <inheritdoc cref="ITaskRecordTextFactory"/>
    internal class TaskRecordTextFactory : ITaskRecordTextFactory
    {
        /// <summary>
        /// Провайдер сервисов
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        public TaskRecordTextFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        /// <inheritdoc/>
        public ITaskRecordText GetTaskRecordText(RecordTypes recordType)
            => recordType switch
            {
                RecordTypes.CreateTask => _serviceProvider.GetRequiredService<TaskCreationRecordText>(),

                RecordTypes.AddTaskDoneFile => _serviceProvider.GetRequiredService<TaskFileAdditionRecordText>(),

                RecordTypes.AddReview => _serviceProvider.GetRequiredService<TaskReviewCreationRecordText>(),

                RecordTypes.DownloadTaskDoneFile => _serviceProvider.GetRequiredService<DownloadDoneTaskFileRecordText>(),

                _ => throw new NotImplementedException()
            };
    }
}