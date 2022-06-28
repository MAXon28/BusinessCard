using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProjectCompatibilityRepository : IRepository<ProjectCompatibility> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <returns>  </returns>
        public Task<IEnumerable<string>> GetCompatibilitiesByProjectIdAsync(int projectId);
    }
}