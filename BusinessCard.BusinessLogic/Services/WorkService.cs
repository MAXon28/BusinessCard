using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Interfaces.Utils;
using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonWork;
using BusinessCard.DataAccessLayer.Entities.Content;
using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using BusinessCard.Entities.DTO.Work;
using DapperAssistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IWorkService"/>
    internal class WorkService : IWorkService
    {
        /// <summary>
        /// Ключ флага
        /// </summary>
        private const string FlagKey = "NeedWork";

        /// <summary>
        /// Количество вакансий в одном пакете
        /// </summary>
        private const int VacanciesCountInPackage = 7;

        /// <summary>
        /// Репозиторий работы
        /// </summary>
        private readonly IWorkRepository _workRepository;

        /// <summary>
        /// Репозиторий флагов приложения
        /// </summary>
        private readonly IFlagRepository _flagRepository;

        /// <summary>
        /// Репозиторий вакансий
        /// </summary>
        private readonly IVacancyRepository _vacancyRepository;

        /// <summary>
        /// Фабрика строителя запроса выборки
        /// </summary>
        private readonly ISelectionQueryBuilderFactory _selectionQueryBuilderFactory;

        /// <summary>
        /// Пагинация
        /// </summary>
        private readonly IPagination _pagination;

        public WorkService(
            IWorkRepository workRepository, 
            IFlagRepository flagRepository,
            IVacancyRepository vacancyRepository, 
            ISelectionQueryBuilderFactory selectionQueryBuilderFactory, 
            IPagination pagination)
        {
            _workRepository = workRepository;
            _flagRepository = flagRepository;
            _vacancyRepository = vacancyRepository;
            _selectionQueryBuilderFactory = selectionQueryBuilderFactory;
            _pagination = pagination;
        }

        /// <inheritdoc/>
        public async Task<Resume> GetResumeAsync()
        {
            if (!await _flagRepository.GetFlagValueAsync(FlagKey))
                return null;

            var data = (await _workRepository.GetAsync()).ToArray()[0];
            return new()
            {
                Id = data.Id,
                Position = data.Position,
                Salary = data.SalaryInfo,
                Schedule = data.Schedule,
                TechnologyStack = data.TechnologyStack
            };
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateFlagAsync(bool value)
            => await _flagRepository.UpdateFlagValueAsync(new Flag
            {
                FlagKey = FlagKey,
                Value = value
            }) == 1;

        /// <inheritdoc/>
        public async Task<bool> UpdateResumeAsync(Resume resume)
            => await _workRepository.UpdateAsync(new Work
            {
                Id = resume.Id,
                Position = resume.Position,
                SalaryInfo = resume.Salary,
                Schedule = resume.Schedule,
                TechnologyStack = resume.TechnologyStack
            }) == 1;

        /// <inheritdoc/>
        public async Task<bool> TryAddVacancyAsync(VacancyDto vacancyDto)
        {
            var vacancy = new Vacancy
            {
                Name = vacancyDto.Name,
                Company = vacancyDto.Company,
                Position = vacancyDto.Position,
                WorkFormat = vacancyDto.WorkFormat,
                Contact = vacancyDto.Contact,
                CreationDate = DateTime.Now.Date,
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Получить данные в правильном формате (выполняется для адреса и для описания вакансии)
        /// </summary>
        /// <param name="data"> Пришедшие данные </param>
        /// <returns> Данные в правильном формате </returns>
        private static string GetDataInTrueFormat(string data)
            => string.IsNullOrEmpty(data) is false && string.IsNullOrWhiteSpace(data) is false
                ? data
                : null;

        /// <summary>
        /// Получить значение зарплаты в правильном формате
        /// </summary>
        /// <param name="salary"> Пришедшее значение зарплаты </param>
        /// <returns> Значение зарплаты в правильном формате </returns>
        private static int? GetSalaryInTrueFormat(string salary)
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
        private static string GetCurrencyInTrueFormat(int? salaryFrom, int? salaryTo, string currency)
            => salaryFrom is not null || salaryTo is not null
                ? currency
                : null;

        /// <inheritdoc/>
        public async Task<VacanciesStatistic> GetVacanciesStatisticAsync()
        {
            var vacanciesStatistic = await _vacancyRepository.GetVacanciesStatisticAsync();
            return new()
            {
                VacanciesCount = vacanciesStatistic.Count,
                VacanciesNotViewedCount = vacanciesStatistic
                    .Where(z => z is false)
                    .Count()
            };
        }

        /// <inheritdoc/>
        public async Task<(IReadOnlyCollection<ShortVacancyInfo> shortVacanciesInfo, int vacanciesCount, int packagesCount)> GetShortVacanciesDataAsync(VacancyFilters vacancyFilters)
        {
            var requestSettings = GetRequestSettings(vacancyFilters);
            var shortVacanciesInfo = GetShortVacanciesInfoAsync(requestSettings);
            var vacanciesCount = vacancyFilters.NeedPackagesCount
                ? await GetVacanciesCountAsync(requestSettings)
                : default;
            var packagesCount = vacanciesCount == default
                ? default
                : _pagination.GetPagesCount(VacanciesCountInPackage, vacanciesCount);
            return (await shortVacanciesInfo, vacanciesCount, packagesCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestSettings">  </param>
        /// <returns>  </returns>
        private async Task<IReadOnlyCollection<ShortVacancyInfo>> GetShortVacanciesInfoAsync(RequestSettings requestSettings)
        {
            var queryBuilder = _selectionQueryBuilderFactory.GetQueryBuilder(QueryBuilderTypes.Vacancies);
            var queryData = queryBuilder.GetQueryData(requestSettings);
            return (await _vacancyRepository.GetShortVacanciesDataAsync(queryData.SqlQuery, queryData.Parameters))
                .Select(x => new ShortVacancyInfo
                {
                    Id = x.Id,
                    Name = x.Name,
                    Company = x.Company,
                    Salary = GetSalaryInfo(x.SalaryFrom, x.SalaryTo, x.Currency),
                    CreationDate = x.CreationDate.ConvertToReadableFormat(),
                    ViewedByMAXon28Team = x.ViewedByMAXon28Team
                })
                .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestSettings">  </param>
        /// <returns>  </returns>
        private async Task<int> GetVacanciesCountAsync(RequestSettings requestSettings)
        {
            var queryBuilder = _selectionQueryBuilderFactory.GetQueryBuilder(QueryBuilderTypes.Vacancies);
            queryBuilder.TypeOfSelect = SelectTypes.Count;
            var queryData = queryBuilder.GetQueryData(requestSettings);
            return await _vacancyRepository.GetVacanciesCountAsync(queryData.SqlQuery, queryData.Parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyFilters">  </param>
        /// <returns>  </returns>
        private static RequestSettings GetRequestSettings(VacancyFilters vacancyFilters)
            => new VacancyRequestSettings(vacancyFilters.LastVacancyId, VacanciesCountInPackage, vacancyFilters.SearchText);

        /// <inheritdoc/>
        public async Task<VacancyInfo> GetVacancyAsync(int vacancyId)
        {
            var querySettings = new QuerySettings
            {
                ConditionField = "Id",
                ConditionFieldValue = vacancyId,
                ConditionType = ConditionType.EQUALLY
            };
            var vacancy = (await _vacancyRepository.GetWithConditionAsync(querySettings)).FirstOrDefault();

            if (vacancy.ViewedByMAXon28Team is false)
                await _vacancyRepository.UpdateVacancyViewedStatusAsync(vacancyId);

            return new()
            {
                Name = vacancy.Name,
                Company = vacancy.Company,
                Position = vacancy.Position,
                WorkFormat = vacancy.WorkFormat,
                Contact = vacancy.Contact,
                CreationDate = vacancy.CreationDate.ConvertToReadableFormat(),
                Salary = GetSalaryInfo(vacancy.SalaryFrom, vacancy.SalaryTo, vacancy.Currency),
                Description = vacancy.Description
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="salaryFrom">  </param>
        /// <param name="salaryTo">  </param>
        /// <param name="currency">  </param>
        /// <returns>  </returns>
        private static string GetSalaryInfo(int? salaryFrom, int? salaryTo, string currency)
            => (salaryFrom is not null, salaryTo is not null, string.IsNullOrEmpty(currency) is false) switch
            {
                (false, false, false) => "-",
                (true, false, true) => $"от {salaryFrom} {currency}",
                (false, true, true) => $"до {salaryTo} {currency}",
                (true, true, true) => $"{salaryFrom} - {salaryTo} {currency}",
                _ => throw new Exception("Неопознанная ошибка")
            };
    }
}