using BusinessCard.BusinessLogicLayer.Interfaces.Utils.TasksRecords;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.TasksRecords.Text;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BusinessCard.BusinessLogicLayer.Utils.TasksRecords.Data
{
    /// <inheritdoc cref="ITaskRecordDataFactory"/>
    internal class TaskRecordDataFactory : ITaskRecordDataFactory
    {
        /// <summary>
        /// Провайдер сервисов
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        public TaskRecordDataFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        /// <inheritdoc/>
        public ITaskRecordData GetTaskRecordData(RecordTypes recordType)
            => recordType switch
            {
                RecordTypes.UpdateStatus => _serviceProvider.GetRequiredService<TaskStatusUpdateRecordData>(),

                RecordTypes.AddPrice => _serviceProvider.GetRequiredService<TaskPriceAdditionRecordData>(),

                RecordTypes.UpdateTaskCounter => _serviceProvider.GetRequiredService<TaskCounterUpdateRecordData>(),

                _ => throw new NotImplementedException()
            };
    }
}