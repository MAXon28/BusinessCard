using BusinessCard.DataAccessLayer.Entities.Data;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.Data
{
    /// <summary>
    /// Репозиторий флагов приложение
    /// </summary>
    public interface IFlagRepository : IRepository<Flag>
    {
        /// <summary>
        /// Получить значение флага
        /// </summary>
        /// <param name="key"> Ключ флага </param>
        /// <returns> Значение флага </returns>
        public Task<bool> GetFlagValueAsync(string key);

        /// <summary>
        /// Обновить значение флага
        /// </summary>
        /// <param name="flag"> Флаг </param>
        /// <returns> Количество обновлённых строк </returns>
        public Task<int> UpdateFlagValueAsync(Flag flag);
    }
}