using BusinessCard.DataAccessLayer.Entities.Data;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.Data
{
    /// <summary>
    /// Репозиторий сущности "Вакансия"
    /// </summary>
    public interface IVacancyRepository : IRepository<Vacancy> 
    {
        /// <summary>
        /// Получить статистику по вакансиям
        /// </summary>
        /// <returns> Статистика по вакансиям </returns>
        public Task<IReadOnlyCollection<bool>> GetVacanciesStatisticAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery">  </param>
        /// <param name="parameters">  </param>
        /// <returns>  </returns>
        public Task<IReadOnlyCollection<Vacancy>> GetShortVacanciesDataAsync(string sqlQuery, DynamicParameters parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery">  </param>
        /// <param name="parameters">  </param>
        /// <returns>  </returns>
        public Task<int> GetVacanciesCountAsync(string sqlQuery, DynamicParameters parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vacancyId">  </param>
        /// <returns></returns>
        public Task UpdateVacancyViewedStatusAsync(int vacancyId);
    }
}