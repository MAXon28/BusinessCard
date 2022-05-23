using BusinessCard.BusinessLogicLayer.DTOs;
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
        public Task<Dictionary<string, string>> GetResumeAsync();

        /// <summary>
        /// Добавить вакансию
        /// </summary>
        /// <param name="vacancyDto"> Данные новой вакансии </param>
        /// <returns> True - если вакансия успешно создалась, False - в случае ошибки </returns>
        public Task<bool> TryAddVacancyAsync(VacancyDto vacancyDto);
    }
}