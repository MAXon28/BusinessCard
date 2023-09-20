using BusinessCard.Entities.DTO.AboutMe;
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
        public Task<IReadOnlyCollection<Fact>> GetFactsAsync();

        /// <summary>
        /// Обновить факт
        /// </summary>
        /// <param name="fact"> Факт </param>
        /// <returns> Успешно прошло обновление или нет </returns>
        public Task<bool> UpdateFactAsync(Fact fact);
    }
}