using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProjectRepository : IRepository<Project> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery">  </param>
        /// <param name="parameters">  </param>
        /// <returns>  </returns>
        public Task<IEnumerable<Project>> GetProjectsAsync(string sqlQuery, DynamicParameters parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery">  </param>
        /// <param name="parameters">  </param>
        /// <returns>  </returns>
        public Task<int> GetProjectsCountAsync(string sqlQuery, DynamicParameters parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<ProjectsInformation> GetProjectInformationAsync();
    }
}