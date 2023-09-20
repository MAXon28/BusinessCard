using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using BusinessCard.Entities.DTO;
using BusinessCard.Entities.DTO.Service;
using DapperAssistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IConditionService"/>
    internal class ConditionService : IConditionService
    {
        /// <summary>
        /// Репозиторий условий сервиса
        /// </summary>
        private readonly IConditionRepository _conditionRepository;

        /// <summary>
        /// Репозиторий вычисляемых значений
        /// </summary>
        private readonly ICalculatedValueRepository _calculatedValueRepository;

        /// <summary>
        /// Репозиторий вычисляемых значений сервиса
        /// </summary>
        private readonly IServiceCalculatedValueRepository _serviceCalculatedValueRepository;

        public ConditionService(IConditionRepository conditionRepository, ICalculatedValueRepository calculatedValueRepository, IServiceCalculatedValueRepository serviceCalculatedValueRepository)
        {
            _conditionRepository = conditionRepository;
            _calculatedValueRepository = calculatedValueRepository;
            _serviceCalculatedValueRepository = serviceCalculatedValueRepository;
        }
        
        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<ConditionDto>> GetConditionsByServiceAsync(int serviceId)
            => (await _conditionRepository.GetWithConditionAsync(new QuerySettings
                {
                    ConditionField = "ServiceId",
                    ConditionFieldValue = serviceId,
                    ConditionType = ConditionType.EQUALLY
                }))
            .Select(x => new ConditionDto
            {
                Id = x.Id,
                Text = x.ConditionText,
                ServiceId = x.ServiceId
            })
            .ToArray();

        /// <inheritdoc/>
        public async Task<bool> AddConditionAsync(ConditionDto condition)
            => await _conditionRepository.AddConditionAsync(new Condition
            {
                ConditionText = condition.Text,
                ServiceId = condition.ServiceId
            });

        /// <inheritdoc/>
        public async Task<bool> UpdateConditionAsync(ConditionDto condition)
            => await _conditionRepository.UpdateAsync(new Condition
            {
                Id = condition.Id,
                ConditionText = condition.Text,
                ServiceId = condition.ServiceId
            }) == 1;

        /// <inheritdoc/>
        public async Task<bool> DeleteConditionAsync(int conditionId)
            => await _conditionRepository.DeleteAsync(conditionId) == 1;

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<CalculatedValueDto>> GetCalculatedValuesForServiceAsync()
            => (await _calculatedValueRepository.GetWithConditionAsync(new QuerySettings
            {
                ConditionField = "ForService",
                ConditionFieldValue = true,
                ConditionType = ConditionType.EQUALLY
            }))
            .Select(x => new CalculatedValueDto
            {
                Id = x.Id,
                Value = x.Value
            })
            .ToArray();

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<ServiceCalculatedValueDto>> GetServiceCalculatedValuesAsync(int serviceId)
            => (await _serviceCalculatedValueRepository.GetWithConditionAsync(new QuerySettings
            {
                ConditionField = "ServiceId",
                ConditionFieldValue = serviceId,
                ConditionType = ConditionType.EQUALLY
            }))
            .Select(x => new ServiceCalculatedValueDto
            {
                Id = x.Id,
                Number = x.Number,
                CalculatedValueId = x.CalculatedValueId,
                Value = x.CalculatedValue.Value
            })
            .ToArray();

        /// <inheritdoc/>
        public async Task<bool> AddServiceCalculatedValueAsync(ServiceCalculatedValueDto serviceCalculatedValue)
            => await _serviceCalculatedValueRepository.AddAsync(new ServiceCalculatedValue
            {
                Number = serviceCalculatedValue.Number,
                CalculatedValueId = serviceCalculatedValue.CalculatedValueId,
                ServiceId = serviceCalculatedValue.ServiceId
            }) == 1;

        /// <inheritdoc/>
        public async Task<bool> UpdateServiceCalculatedValueAsync(ServiceCalculatedValueDto serviceCalculatedValue)
            => await _serviceCalculatedValueRepository.UpdateAsync(new ServiceCalculatedValue
            {
                Id = serviceCalculatedValue.Id,
                Number = serviceCalculatedValue.Number,
                CalculatedValueId = serviceCalculatedValue.CalculatedValueId,
                ServiceId = serviceCalculatedValue.ServiceId
            }) == 1;

        /// <inheritdoc/>
        public async Task<bool> DeleteServiceCalculatedValueAsync(int serviceCalculatedValueId)
            => await _serviceCalculatedValueRepository.DeleteAsync(serviceCalculatedValueId) == 1;
    }
}