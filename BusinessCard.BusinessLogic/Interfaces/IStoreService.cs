using BusinessCard.Entities.DTO.Store;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStoreService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters">  </param>
        /// <param name="projectsPackageNumber">  </param>
        /// <returns>  </returns>
        public Task<List<ProjectInformation>> GetProjectsAsync(FiltersIn filters, int projectsPackageNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters">  </param>
        /// <param name="projectsPackageNumber">  </param>
        /// <returns>  </returns>
        public Task<ProjectsInformationDto> GetProjectsInformationAsync(FiltersIn filters, int projectsPackageNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<FiltersOut> GetFiltersAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<GeneralInformation> GetGeneralInformationAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <returns>  </returns>
        public Task<ProjectInformation> GetProjectInformationAsync(int projectId);
    }
}