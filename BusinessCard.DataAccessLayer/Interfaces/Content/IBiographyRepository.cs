using BusinessCard.DataAccessLayer.Entities.Content;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.Content
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBiographyRepository : IRepository<Biography>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<IEnumerable<Biography>> GetSortedBiographyByPriorityAsync();
    }
}