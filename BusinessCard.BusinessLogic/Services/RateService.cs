using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using BusinessCard.Entities.DTO.Service;
using DapperAssistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IRateService"/>
    internal class RateService : IRateService
    {
        /// <summary>
        /// Репозиторий тарифов сервисов
        /// </summary>
        private readonly IRateRepository _rateRepository;

        /// <summary>
        /// Репозиторий значений условий сервисов
        /// </summary>
        private readonly IConditionValueRepository _conditionValueRepository;

        public RateService(IRateRepository rateRepository, IConditionValueRepository conditionValueRepository)
        {
            _rateRepository = rateRepository;
            _conditionValueRepository = conditionValueRepository;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<MAXonVersionRate>> GetRatesAsync(int serviceId)
            => (await _rateRepository.GetRatesAsync(serviceId))
                .Select(x => new MAXonVersionRate
                {
                    Id = x.Id,
                    Name = x.Name,
                    ServiceCounterId = x.ServiceCounterId
                })
                .ToArray();

        /// <inheritdoc/>
        public async Task<MAXonVersionRate> GetRateAsync(int rateId)
        {
            var rate = (await _rateRepository.GetWithoutJoinAsync(new QuerySettings
            {
                ConditionField = "Id",
                ConditionFieldValue = rateId,
                ConditionType = ConditionType.EQUALLY
            })).First();

            var conditions = await _conditionValueRepository.GetWithConditionAsync(new QuerySettings
            {
                ConditionField = "RateId",
                ConditionFieldValue = rateId,
                ConditionType = ConditionType.EQUALLY
            });

            return new()
            {
                Name = rate.Name,
                Description = rate.Description,
                Price = rate.Price,
                IsSpecificPrice = rate.IsSpecificPrice,
                IsPublic = rate.IsPublic,
                Conditions = conditions
                    .Select(x => new ConditionInfo
                    {
                        ConditionValue = x.Condition.ConditionText,
                        ConditionValueId = x.Id,
                        ConditionValueText = x.Value == "+" || x.Value == "-" ? string.Empty : x.Value,
                        IsAvailable = x.Value != "-"
                    })
                    .ToArray()
            };
        }

        /// <inheritdoc/>
        public async Task<bool> AddRateAsync(MAXonVersionRate rate)
        {
            var newRateId = await _rateRepository.AddAsync<int>(GetRateForSave(rate));
            return await _conditionValueRepository.AddConditionValues(GetConditionValuesForSave(rate.Conditions, newRateId)) == rate.Conditions.Count;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateRateAsync(MAXonVersionRate rate, int updateType)
        {
            const int verifyUpdateRateDetailCounter = 1;
            var verifyUpdateRateConditions = rate.Conditions is not null ? rate.Conditions.Count : 0;

            var (needUpdateRateDetail, needUpdateRateConditions) = updateType.ToEnum<UpdateTypes>().GetSettingsForUpdate();

            var tasksBeforeUpdate = new List<Task<bool>>();

            if (needUpdateRateDetail)
                tasksBeforeUpdate.Add(
                    CheckUpdateAsync(
                        _rateRepository.UpdateRateAsync(GetRateForSave(rate)),
                        verifyUpdateRateDetailCounter));

            if (needUpdateRateConditions)
                tasksBeforeUpdate.Add(
                    CheckUpdateAsync(
                        _conditionValueRepository.UpdateConditionValues(GetConditionValuesForSave(rate.Conditions)),
                        verifyUpdateRateConditions));

            foreach (var taskBeforeUpdate in tasksBeforeUpdate)
                if (!await taskBeforeUpdate)
                    return false;

            return true;
        }

        /// <summary>
        /// Получить данные по тарифу для сохранения
        /// </summary>
        /// <param name="rate"> Данные по тарифу </param>
        /// <returns> Данные по тарифу для сохранения </returns>
        private static Rate GetRateForSave(MAXonVersionRate rate)
            => new()
            {
                Id = rate.Id,
                Name = rate.Name,
                Description = rate.Description,
                Price = rate.Price,
                IsSpecificPrice = rate.IsSpecificPrice,
                IsPublic = rate.IsPublic,
                ServiceId = rate.ServiceId
            };

        /// <summary>
        /// Получить данные по значениям условий тарифа для сохранения
        /// </summary>
        /// <param name="conditionValues"> Данные по значениям условий тарифа </param>
        /// <param name="rateId"> Идентификатор тарифа </param>
        /// <returns> Данные по значениям условий тарифа для сохранения </returns>
        private static IEnumerable<ConditionValue> GetConditionValuesForSave(IReadOnlyCollection<ConditionInfo> conditionValues, int rateId)
            => conditionValues
                .Select(x => new ConditionValue
                {
                    Id = x.ConditionValueId,
                    Value = GetConditionValueForSave(x.IsAvailable, x.ConditionValueText),
                    ConditionId = x.ConditionId,
                    RateId = rateId
                });

        /// <summary>
        /// Получить данные по значениям условий тарифа для сохранения
        /// </summary>
        /// <param name="conditionValues"> Данные по значениям условий тарифа </param>
        /// <returns> Данные по значениям условий тарифа для сохранения </returns>
        private static IEnumerable<ConditionValue> GetConditionValuesForSave(IReadOnlyCollection<ConditionInfo> conditionValues)
            => conditionValues
                .Select(x => new ConditionValue
                {
                    Id = x.ConditionValueId,
                    Value = GetConditionValueForSave(x.IsAvailable, x.ConditionValueText)
                });

        /// <summary>
        /// Получить значения условия для сохранения
        /// </summary>
        /// <param name="isAvailable"> Доступно это условие или нет </param>
        /// <param name="conditionValueText"> Текст значения условия </param>
        /// <returns> Значения условия для сохранения </returns>
        private static string GetConditionValueForSave(bool isAvailable, string conditionValueText)
            => (isAvailable, !string.IsNullOrEmpty(conditionValueText)) switch
            {
                (false, false) => "-",
                (false, true) => "-",
                (true, false) => "+",
                (true, true) => conditionValueText
            };

        /// <summary>
        /// Проверить правильность обновления
        /// </summary>
        /// <param name="updateTask"> Процесс обновления </param>
        /// <param name="verifyNumber"> Проверямое число - количество значений, которые должны быть обновлены </param>
        /// <returns> Данные обновлены правильно </returns>
        private static async Task<bool> CheckUpdateAsync(Task<int> updateTask, int verifyNumber)
            => await updateTask == verifyNumber;

        /// <inheritdoc/>
        public async Task<bool> UpdateRateCalculatedValueAsync(int rateId, int? serviceCounterId)
            => await _rateRepository.UpdateRateCalculatedValueAsync(rateId, serviceCounterId) == 1;
    }
}