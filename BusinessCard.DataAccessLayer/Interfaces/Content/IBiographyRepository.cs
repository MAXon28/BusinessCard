using BusinessCard.DataAccessLayer.Entities.Content;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.Content
{
    /// <summary>
    /// Репозиторий биографии
    /// </summary>
    public interface IBiographyRepository : IRepository<Biography>
    {
        /// <summary>
        /// Обновить биографию
        /// </summary>
        /// <param name="text"> Текст биографии </param>
        /// <returns> Количество обновлённых строк </returns>
        public Task<int> UpdateBiographyAsync(string text);
    }
}