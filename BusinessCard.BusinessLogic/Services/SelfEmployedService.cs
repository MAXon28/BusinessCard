using BusinessCard.BusinessLogicLayer.BusinessModels;
using BusinessCard.BusinessLogicLayer.DTOs;
using BusinessCard.BusinessLogicLayer.DTOs.Service;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Entities.Content.Services;
using BusinessCard.DataAccessLayer.Interfaces.Content.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessCard.BusinessLogicLayer.Services
{
    public class SelfEmployedService : ISelfEmployedService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceRepository _serviceRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IShortDescriptionRepository _shortDescriptionRepository;

        /// <summary>
        /// 
        /// </summary>
        private IRateRepository _rateRepository;

        /// <summary>
        /// 
        /// </summary>
        private ServiceConfiguration _serviceConfiguration;

        public SelfEmployedService(IServiceRepository serviceRepository, IShortDescriptionRepository shortDescriptionRepository, IRateRepository rateRepository)
        {
            _serviceRepository = serviceRepository;
            _shortDescriptionRepository = shortDescriptionRepository;
            _rateRepository = rateRepository;
        }

        public async Task<List<ServiceDto>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAsync();

            var shortDescriptions = await _shortDescriptionRepository.GetAsync();

            var servicesDto = new List<ServiceDto>();

            foreach (var service in services)
            {
                var serviceDto = new ServiceDto
                {
                    Id = service.Id,
                    Name = service.Name,
                    Price = $"От {service.Price} ₽ за работу",
                    ShortDescriptions = shortDescriptions.Where(d => d.ServiceId == service.Id).Select(d => d.Data).ToList()
                };

                servicesDto.Add(serviceDto);
            }

            return servicesDto;
        }

        public async Task<string> GetFullDescriptionAsync(int serviceId) => await _serviceRepository.GetFullDescriptionByServiceId(serviceId);

        public async Task<List<Review>> GetFortyReviews(int serviceId)
        {
            var reviews = new List<Review>
            {
                new Review
                {
                    UserName = "Тестер 5",
                    Text = "Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5.",
                    Rating = 5,
                    Date = "29 декабря 2021"
                },
                new Review
                {
                    UserName = "Тестер 1",
                    Text = "Тест 1. Тест 1. Тест 1.",
                    Rating = 5,
                    Date = "Вчера"
                },
                new Review 
                {
                    UserName = "Тестер 2",
                    Text = "Тест 2. Тест 2. Тест 2. Тест 2. Тест 2.",
                    Rating = 4,
                    Date = "5 января"
                },
                new Review
                {
                    UserName = "Тестер 3",
                    Text = "Тест 3. Тест 3. Тест 3. Тест 3. Тест 3. Тест 3. Тест 3. Тест 3. Тест 3.",
                    Rating = 5,
                    Date = "3 января"
                },
                new Review
                {
                    UserName = "Тестер 4",
                    Text = "Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4. Тест 4.",
                    Rating = 5,
                    Date = "2 января"
                },
                new Review
                {
                    UserName = "Тестер 5",
                    Text = "Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5. Тест 5.",
                    Rating = 5,
                    Date = "29 декабря 2021"
                }
            };

            return reviews;
        }

        public async Task<AdvancedServiceDto> GetAdvancedService(int serviceId)
        {
            var service = await _serviceRepository.GetService(serviceId);

            if (service is null)
                throw new Exception("");

            SetServiceConfiguration();

            var defaultPrice = GetPriceInformation(service.Price, service.ConcretePrice);

            var advancedService = new AdvancedServiceDto(service.Id, service.Name, defaultPrice, service.NeedTechnicalSpecification, service.PrePrice, service.PreDeadline);

            if (service.Rates.Count > 0)
            {
                foreach (var rate in service.Rates.Values)
                    advancedService.Rates.Add(GetRateDto(rate));
            }

            return advancedService;
        }

        /// <summary>
        /// Задать настройки сервиса
        /// </summary>
        private void SetServiceConfiguration()
        {
            const string configurationFileName = "wwwroot/BusinessConfigurations/ServiceConfiguration.xml";

            try
            {
                using var fileStream = new FileStream(configurationFileName, FileMode.Open);

                var formatter = new XmlSerializer(typeof(ServiceConfiguration));

                _serviceConfiguration = (ServiceConfiguration)formatter.Deserialize(fileStream);
            }
            catch
            {
                //
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="price">  </param>
        /// <param name="concretePrice">  </param>
        /// <returns>  </returns>
        private string GetPriceInformation(int price, bool concretePrice)
        {
            var template = concretePrice ? _serviceConfiguration.ConcretePriceTemplate : _serviceConfiguration.NotConcretePriceTemplate;

            return template.Replace(_serviceConfiguration.PriceTemplate, price.ToString()).Replace(_serviceConfiguration.CurrencyTemplate, _serviceConfiguration.Currency);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rate">  </param>
        /// <returns>  </returns>
        private RateDto GetRateDto(Rate rate)
        {
            var ratePrice = GetPriceInformation(rate.Price, rate.IsSpecificPrice);
            var rateDto = new RateDto(rate.Id, rate.Name, rate.Description, ratePrice);

            foreach (var conditionValue in rate.ConditionsValues)
            {
                var condition = conditionValue.Condition.ConditionText;

                var conditionValueDto = GetConditionValueDto(conditionValue);

                rateDto.Conditions.Add(condition, conditionValueDto);
            }

            return rateDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionValue">  </param>
        /// <returns>  </returns>
        private ConditionValueDto GetConditionValueDto(ConditionValue conditionValue)
        {
            if (conditionValue.Value == "+")
                return new ConditionValueDto(true);

            if (conditionValue.Value == "-")
                return new ConditionValueDto(false);

            return new ConditionValueDto(true, conditionValue.Value);
        }
    }
}