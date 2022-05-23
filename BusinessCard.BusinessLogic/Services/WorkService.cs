using BusinessCard.BusinessLogicLayer.BusinessModels;
using BusinessCard.BusinessLogicLayer.DTOs;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using DapperAssistant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IWorkService"/>
    public class WorkService : IWorkService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IWorkRepository _workRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IVacancyRepository _vacancyRepository;

        public WorkService(IWorkRepository workRepository, IVacancyRepository vacancyRepository)
        {
            _workRepository = workRepository;
            _vacancyRepository = vacancyRepository;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, string>> GetResumeAsync()
        {
            var resumeConfiguration = GetResumeConfiguration();

            if (resumeConfiguration is null || !resumeConfiguration.NeedWork)
                return null;

            var querySettings = new QuerySettings
            {
                ConditionField = "Id",
                ConditionType = ConditionType.EQUALLY,
                ConditionFieldValue = resumeConfiguration.CurrentResumeId
            };

            try
            {
                var data = (await _workRepository.GetWithConditionAsync(querySettings)).ToList()[0];

                return new Dictionary<string, string>
                {
                    ["Position"] = data.Position,
                    ["Salary"] = $"От {data.Salary} ₽ (на руки)",
                    ["Schedule"] = data.Schedule,
                    ["TechnologyStack"] = data.TechnologyStack
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> TryAddVacancyAsync(VacancyDto vacancyDto)
        {
            var vacancy = new Vacancy
            {
                Name = vacancyDto.Name,
                Company = vacancyDto.Company,
                Position = vacancyDto.Position,
                Schedule = vacancyDto.Schedule,
                Contacts = vacancyDto.Contacts,
                Address = GetDataInTrueFormat(vacancyDto.Address),
                SalaryFrom = GetSalaryInTrueFormat(vacancyDto.SalaryFrom),
                SalaryTo = GetSalaryInTrueFormat(vacancyDto.SalaryTo),
                Description = GetDataInTrueFormat(vacancyDto.Description)
            };
            vacancy.Currency = GetCurrencyInTrueFormat(vacancy.SalaryFrom, vacancy.SalaryTo, vacancyDto.Currency);

            try
            {
                await _vacancyRepository.AddAsync(vacancy);

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Получить настройки резюме
        /// </summary>
        /// <returns> Настройки резюме </returns>
        private ResumeConfiguration GetResumeConfiguration()
        {
            const string configurationFileName = "wwwroot/BusinessConfigurations/ResumeConfiguration.xml";

            try
            {
                using var fileStream = new FileStream(configurationFileName, FileMode.Open);

                var formatter = new XmlSerializer(typeof(ResumeConfiguration));

                return (ResumeConfiguration)formatter.Deserialize(fileStream);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Получить данные в правильном формате (выполняется для адреса и для описания вакансии)
        /// </summary>
        /// <param name="data"> Пришедшие данные </param>
        /// <returns> Данные в правильном формате </returns>
        private string GetDataInTrueFormat(string data) => string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data) ? null : data;

        /// <summary>
        /// Получить значение зарплаты в правильном формате
        /// </summary>
        /// <param name="salary"> Пришедшее значение зарплаты </param>
        /// <returns> Значение зарплаты в правильном формате </returns>
        private int? GetSalaryInTrueFormat(string salary)
        {
            if (int.TryParse(salary, out int salaryInTrueFormat))
                return salaryInTrueFormat;

            return null;
        }

        /// <summary>
        /// Получить значение валюты в правильном формате
        /// </summary>
        /// <param name="salaryFrom"> Значение зарплаты (От) </param>
        /// <param name="salaryTo"> Значение зарплаты (До) </param>
        /// <param name="currency"> Пришедшее значение валюты </param>
        /// <returns> Значение валюты в правильном формате </returns>
        private string GetCurrencyInTrueFormat(int? salaryFrom, int? salaryTo, string currency) => salaryFrom is null && salaryTo is null ? null : currency;
    }
}