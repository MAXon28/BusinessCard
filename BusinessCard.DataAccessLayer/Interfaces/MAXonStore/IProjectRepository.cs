using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Repositories.MAXonStore.QueryHelper;
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
        /// <param name="projectQuerySettings">  </param>
        /// <returns>  </returns>
        public Task<IEnumerable<Project>> GetProjectsAsync(ProjectQuerySettings projectQuerySettings);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectQuerySettings">  </param>
        /// <returns>  </returns>
        public Task<int> GetProjectsCountAsync(ProjectQuerySettings projectQuerySettings);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<ProjectInformation> GetProjectInformationAsync();
    }
}