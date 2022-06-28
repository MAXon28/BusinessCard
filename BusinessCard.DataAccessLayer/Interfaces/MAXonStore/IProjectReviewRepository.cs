using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonStore
{
    /// <summary>
    /// Репозиторий отзывов проекта
    /// </summary>
    public interface IProjectReviewRepository : IRepository<ProjectReview> 
    {
        public Task<IEnumerable<ProjectReview>> GetReviewsAsync(int projectId, int currentReviewId);

        /// <summary>
        /// Получить статистику отзывов по конкретному проектому
        /// </summary>
        /// <param name="projectId"> Идентификатор проекта </param>
        /// <returns> Статистика отзывов по проекту </returns>
        public Task<Dictionary<int, int>> GetReviewStatisticAsync(int projectId);
    }
}