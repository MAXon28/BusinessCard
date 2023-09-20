using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using BusinessCard.DataAccessLayer.Entities.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using BusinessCard.Entities.DTO.Review;
using BusinessCard.Entities.DTO.Service;
using DapperAssistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="ISelfEmployedService"/>
    internal class SelfEmployedService : ISelfEmployedService
    {
        /// <summary>
        /// Репозиторий услуг (сервисов)
        /// </summary>
        private readonly IServiceRepository _serviceRepository;

        /// <summary>
        /// Репозиторий краткой информации по сервисам
        /// </summary>
        private readonly IShortDescriptionRepository _shortDescriptionRepository;

        public SelfEmployedService(
            IServiceRepository serviceRepository,
            IShortDescriptionRepository shortDescriptionRepository)
        {
            _serviceRepository = serviceRepository;
            _shortDescriptionRepository = shortDescriptionRepository;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<ServiceInfo>> GetAllPublicServicesAsync()
        {
            var services = await _serviceRepository.GetWithConditionAsync(new QuerySettings
                {
                    ConditionField = "IsPublic",
                    ConditionFieldValue = true,
                    ConditionType = ConditionType.EQUALLY
                });

            var shortDescriptions = await _shortDescriptionRepository.GetShortDescriptionsForPublicServicesAsync();

            return services
                .Select(x => new ServiceInfo
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = GetPriceInformation(x.Price, x.ConcretePrice),
                    ShortDescriptions = shortDescriptions.Where(z => z.ServiceId == x.Id).Select(d => d.Data).ToArray()
                })
                .ToArray();
        }

        /// <inheritdoc/>
        public async Task<string> GetFullDescriptionAsync(int serviceId)
            => await _serviceRepository.GetFullDescriptionByServiceId(serviceId);

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<ReviewData>> GetReviewsAsync(int serviceId)
        {
            var reviews = new List<ReviewData>
            {
                new ReviewData
                {
                    UserName = "Тестер 5",
                    Text = "Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5.",
                    Rating = 5,
                    Date = "29 декабря 2021"
                },
                new ReviewData
                {
                    UserName = "Тестер 1",
                    Text = "Тест 1. Тест 1. Тест 1.",
                    Rating = 5,
                    Date = "Вчера"
                },
                new ReviewData
                {
                    UserName = "Тестер 2",
                    Text = "Тест 2. Тест 2. Тест 2. Тест 2. Тест 2.",
                    Rating = 4,
                    Date = "5 января"
                },
                new ReviewData
                {
                    UserName = "Тестер 3",
                    Text = "Тест 3. Тест 3. Тест 3. Тест 3. Тест 3. Тест 3. Тест 3. Тест 3. Тест 3.",
                    Rating = 5,
                    Date = "3 января"
                },
                new ReviewData
                {
                    UserName = "Тестер 4",
                    Text = "Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4.",
                    Rating = 5,
                    Date = "2 января"
                },
                new ReviewData
                {
                    UserName = "Тестер 5",
                    Text = "Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5.",
                    Rating = 5,
                    Date = "29 декабря 2021"
                }
            };

            return reviews;
        }

        /// <inheritdoc/>
        public async Task<bool> AddServiceAsync(ServiceDetailInfo serviceDetailInfo)
        {
            var serviceId = await _serviceRepository.AddAsync<int>(GetServiceForSave(serviceDetailInfo));
            return await _shortDescriptionRepository.AddShortDescriptionsAsync(GetShortDescriptionsForSave(serviceDetailInfo, serviceId)) == 3;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateServiceAsync(ServiceDetailInfo serviceDetailInfo, int updateType)
        {
            const int verifyUpdateServiceDetailCounter = 1;
            const int verifyUpdateShortDescriptions = 3;

            var (needUpdateServiceDetail, needUpdateShortDescriptions) = updateType.ToEnum<UpdateTypes>().GetSettingsForUpdate();

            var tasksBeforeUpdate = new List<Task<bool>>();

            if (needUpdateServiceDetail)
                tasksBeforeUpdate.Add(
                    CheckUpdateAsync(
                        _serviceRepository.UpdateAsync(GetServiceForSave(serviceDetailInfo)), 
                        verifyUpdateServiceDetailCounter));

            if (needUpdateShortDescriptions)
                tasksBeforeUpdate.Add(
                    CheckUpdateAsync(
                        _shortDescriptionRepository.UpdateShortDescriptionsAsync(GetShortDescriptionsForSave(serviceDetailInfo)), 
                        verifyUpdateShortDescriptions));

            foreach (var taskBeforeUpdate in tasksBeforeUpdate)
                if (!await taskBeforeUpdate)
                    return false;

            return true;
        }

        /// <summary>
        /// Получить данные по сервису для сохранения
        /// </summary>
        /// <param name="serviceDetailInfo"> Детали по сервису </param>
        /// <returns> Данные по сервису для сохранения </returns>
        private static Service GetServiceForSave(ServiceDetailInfo serviceDetailInfo)
            => new()
            {
                Id = serviceDetailInfo.Id,
                Name = serviceDetailInfo.Name,
                FullDescription = serviceDetailInfo.Description,
                ConcretePrice = serviceDetailInfo.ConcretePrice,
                Price = serviceDetailInfo.Price,
                NeedTechnicalSpecification = serviceDetailInfo.NeedTechnicalSpecification,
                PrePrice = serviceDetailInfo.PrePrice,
                PreDeadline = serviceDetailInfo.PreDeadline,
                IsPublic = serviceDetailInfo.IsPublic
            };

        /// <summary>
        /// Получить краткие описания по услуге (сервису) для сохранения
        /// </summary>
        /// <param name="serviceDetailInfo"> Детали по сервису </param>
        /// <param name="serviceId"> Идентификатор сервиса </param>
        /// <returns> Краткие описания по услуге (сервису) для сохранения </returns>
        private static IReadOnlyCollection<ShortDescription> GetShortDescriptionsForSave(ServiceDetailInfo serviceDetailInfo, int serviceId = -1)
            => serviceDetailInfo.ShortDescriptions
                .Select(x => new ShortDescription
                {
                    Id = x.Id,
                    Data = x.Text,
                    ServiceId = serviceId
                })
                .ToArray();

        /// <summary>
        /// Проверить правильность обновления
        /// </summary>
        /// <param name="updateTask"> Процесс обновления </param>
        /// <param name="verifyNumber"> Проверямое число - количество значений, которые должны быть обновлены </param>
        /// <returns> Данные обновлены правильно </returns>
        private static async Task<bool> CheckUpdateAsync(Task<int> updateTask, int verifyNumber)
            => await updateTask == verifyNumber;

        /// <inheritdoc/>
        public async Task<AdvancedServiceDto> GetAdvancedServiceAsync(int serviceId)
        {
            var service = await _serviceRepository.GetService(serviceId);

            var defaultPrice = GetPriceInformation(service.Price, service.ConcretePrice);

            var rates = service.Rates.Count > 0
                ? service.Rates.Values.Select(x => GetRate(x)).ToArray()
                : Array.Empty<UserVersionRate>();

            return new()
            {
                ServiceId = service.Id,
                ServiceName = service.Name,
                DefaultPrice = defaultPrice,
                NeedTechnicalSpecification = service.NeedTechnicalSpecification,
                PrePrice = service.PrePrice,
                PreDeadline = service.PreDeadline,
                Rates = rates
            };
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<ServiceDto>> GetAllServicesAsync()
            => (await _serviceRepository.GetAsync())
                .Select(x => new ServiceDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToArray();

        /// <inheritdoc/>
        public async Task<ServiceDetailInfo> GetServiceDetailInfoAsync(int serviceId)
        {
            var service = (await _serviceRepository.GetWithConditionAsync(new QuerySettings
            {
                ConditionField = "Id",
                ConditionFieldValue = serviceId,
                ConditionType = ConditionType.EQUALLY
            })).FirstOrDefault();

            var shortDescriptions = await _shortDescriptionRepository.GetWithConditionAsync(new QuerySettings
            {
                ConditionField = "ServiceId",
                ConditionFieldValue = serviceId,
                ConditionType = ConditionType.EQUALLY
            });

            return new()
            {
                Name = service.Name,
                Description = service.FullDescription,
                ConcretePrice = service.ConcretePrice,
                Price = service.Price,
                NeedTechnicalSpecification = service.NeedTechnicalSpecification,
                PrePrice = service.PrePrice,
                PreDeadline = service.PreDeadline,
                IsPublic = service.IsPublic,
                ShortDescriptions = shortDescriptions
                    .Select(x => new ShortDescriptionData
                    {
                        Id = x.Id,
                        Text = x.Data
                    })
                    .ToArray()
            };
        }

        /// <summary>
        /// Получить информацию по цене в удобочитаемом формате
        /// </summary>
        /// <param name="price"> Цена </param>
        /// <param name="concretePrice"> Цена фиксированная или нет </param>
        /// <returns> Информация по цене в удобочитаемом формате </returns>
        private string GetPriceInformation(int price, bool concretePrice)
            => concretePrice
                ? $"{price} ₽"
                : $"От {price} ₽ за работу";

        /// <summary>
        /// Получить данные по тарифу для передачи пользователю
        /// </summary>
        /// <param name="rate"> Данные по тарифу </param>
        /// <returns> Данные по тарифу для передачи пользователю </returns>
        private UserVersionRate GetRate(Rate rate)
        {
            var ratePrice = GetPriceInformation(rate.Price, rate.IsSpecificPrice);
            var rateDto = new UserVersionRate
            {
                Id = rate.Id,
                Name = rate.Name,
                Description = rate.Description,
                Price = ratePrice,
                Conditions = new Dictionary<string, ConditionValueDto>()
            };

            foreach (var conditionValue in rate.ConditionsValues)
            {
                var condition = conditionValue.Condition.ConditionText;

                var conditionValueDto = GetConditionValueDto(conditionValue);

                rateDto.Conditions.Add(condition, conditionValueDto);
            }

            return rateDto;
        }

        /// <summary>
        /// Получить значение условия для передачи пользователю
        /// </summary>
        /// <param name="conditionValue"> Значение условия </param>
        /// <returns> Значение условия для передачи пользователю </returns>
        private static ConditionValueDto GetConditionValueDto(ConditionValue conditionValue)
            => conditionValue.Value switch
            {
                "+" => new(true),
                "-" => new(false),
                _ => new(true, conditionValue.Value)
            };
    }
}