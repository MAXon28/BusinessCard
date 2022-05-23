using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class VacancyRepository : StandardRepository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(DbConnectionKeeper dbConnectionKeeper) : base (dbConnectionKeeper) { }
    }
}