using BusinessCard.BusinessLogicLayer.DTOs.AboutMeDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Сервис информации обо мне
    /// </summary>
    public interface IAboutMeService
    {
        /// <summary>
        /// Получить информацию обо мне
        /// </summary>
        /// <returns> Словарь данных обо мне в соответствии с типом </returns>
        public Task<Dictionary<string, List<object>>> GetInformationAboutMe();
    }
}