using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.Entities.DTO.Work;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Сервис работы
    /// </summary>
    public interface IWorkService
    {
        /// <summary>
        /// Получить резюме
        /// </summary>
        /// <returns> Резюме </returns>
        public Task<Resume> GetResumeAsync();

        /// <summary>
        /// Обновить флаг
        /// </summary>
        /// <param name="value"> Значение флага </param>
        /// <returns> True - при успешном обновлении, False - в случае ошибки </returns>
        public Task<bool> UpdateFlagAsync(bool value);

        /// <summary>
        /// Обновить резюме
        /// </summary>
        /// <param name="resume"> Резюме </param>
        /// <returns> True - при успешном обновлении, False - в случае ошибки </returns>
        public Task<bool> UpdateResumeAsync(Resume resume);

        /// <summary>
        /// Добавить вакансию
        /// </summary>
        /// <param name="vacancyDto"> Данные новой вакансии </param>
        /// <returns> True - если вакансия успешно создалась, False - в случае ошибки </returns>
        public Task<bool> TryAddVacancyAsync(VacancyDto vacancyDto);

        /// <summary>
        /// Получить статистику по вакансиям
        /// </summary>
        /// <returns> Статистика по вакансиям </returns>
        public Task<VacanciesStatistic> GetVacanciesStatisticAsync();

        /// <summary>
        /// Получить краткие данные по вакансиям
        /// </summary>
        /// <param name="vacancyFilters"> Фильтры вакансий </param>
        /// <returns> Краткие данные по вакансиям, количество вакансий с учётом фильтров, количество пакетов с вакансиям с учётом фильтров </returns>
        public Task<(IReadOnlyCollection<ShortVacancyInfo> shortVacanciesInfo, int vacanciesCount, int packagesCount)> GetShortVacanciesDataAsync(VacancyFilters vacancyFilters);

        /// <summary>
        /// Получить данные по вакансии
        /// </summary>
        /// <param name="vacancyId"> Идентификатор вакансии </param>
        /// <returns> Данные по вакансии </returns>
        public Task<VacancyInfo> GetVacancyAsync(int vacancyId);
    }
}