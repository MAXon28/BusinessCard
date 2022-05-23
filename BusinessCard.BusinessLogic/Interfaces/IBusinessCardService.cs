using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Сервис визитной карточки
    /// </summary>
    public interface IBusinessCardService
    {
        /// <summary>
        /// Получить список фактов
        /// </summary>
        /// <returns> Список фактов </returns>
        public Task<List<string>> GetFactsAsync();
    }
}