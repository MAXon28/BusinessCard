using BusinessCard.DataAccessLayer.Entities.Data;
using DapperAssistant;

namespace BusinessCard.DataAccessLayer.Interfaces.Data
{
    /// <summary>
    /// Репозиторий вычисляемых значений
    /// </summary>
    public interface ICalculatedValueRepository : IRepository<CalculatedValue> { }
}