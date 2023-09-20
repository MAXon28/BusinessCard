using BusinessCard.DataAccessLayer.Entities.MAXonService;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonService
{
    /// <summary>
    /// Репозиторий условий сервиса
    /// </summary>
    public interface IConditionRepository : IRepository<Condition> 
    {
        /// <summary>
        /// Добавить условие
        /// </summary>
        /// <param name="condition"> Данные условия </param>
        /// <returns> Добавлено ли условие или нет </returns>
        public Task<bool> AddConditionAsync(Condition condition);
    }
}