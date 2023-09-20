using BusinessCard.DataAccessLayer.Entities.MAXonService;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonService
{
    /// <summary>
    /// Репозиторий значений условия сервисов
    /// </summary>
    public interface IConditionValueRepository : IRepository<ConditionValue>
    {
        /// <summary>
        /// Добавить значения условий сервиса
        /// </summary>
        /// <param name="conditionValues"> Значения условий сервиса </param>
        /// <returns> Количество добавленных значений </returns>
        public Task<int> AddConditionValues(IEnumerable<ConditionValue> conditionValues);

        /// <summary>
        /// Обновить значения условий сервиса
        /// </summary>
        /// <param name="conditionValues"> Значения условий сервиса </param>
        /// <returns> Количество обновлённых значений </returns>
        public Task<int> UpdateConditionValues(IEnumerable<ConditionValue> conditionValues);
    }
}