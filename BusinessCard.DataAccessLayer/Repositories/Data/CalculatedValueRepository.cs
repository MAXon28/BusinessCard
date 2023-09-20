using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Repositories.Data
{
    /// <inheritdoc cref="ICalculatedValueRepository"/>
    internal class CalculatedValueRepository : StandardRepository<CalculatedValue>, ICalculatedValueRepository
    {
        public CalculatedValueRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }
    }
}