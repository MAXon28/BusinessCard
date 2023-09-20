using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.Data
{
    /// <inheritdoc cref="IVacancyRepository"/>
    internal class VacancyRepository : StandardRepository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(DbConnectionKeeper dbConnectionKeeper) : base (dbConnectionKeeper) { }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<bool>> GetVacanciesStatisticAsync()
        {
            const string sqlQuery = "SELECT ViewedByMAXon28Team FROM Vacancies";
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return (await dbConnection.QueryAsync<bool>(sqlQuery)).ToArray();
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Vacancy>> GetShortVacanciesDataAsync(string sqlQuery, DynamicParameters parameters)
        {
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return (await dbConnection.QueryAsync<Vacancy>(sqlQuery, parameters)).ToArray();
        }

        /// <inheritdoc/>
        public async Task<int> GetVacanciesCountAsync(string sqlQuery, DynamicParameters parameters)
        {
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.QuerySingleAsync<int>(sqlQuery, parameters);
        }

        /// <inheritdoc/>
        public async Task UpdateVacancyViewedStatusAsync(int vacancyId)
        {
            const string sqlQuery = @"UPDATE Vacancies
                                      SET ViewedByMAXon28Team = 1
                                      WHERE Id = @vacancyId";
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            await dbConnection.ExecuteAsync(sqlQuery, new { vacancyId });
        }
    }
}