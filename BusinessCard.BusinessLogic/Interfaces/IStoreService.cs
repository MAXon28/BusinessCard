using BusinessCard.BusinessLogicLayer.DTOs.Store;
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
        public Task<List<ProjectDto>> GetProjectsAsync(FiltersDtoIn filters, int projectsPackageNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters">  </param>
        /// <param name="projectsPackageNumber">  </param>
        /// <returns>  </returns>
        public Task<ProjectsInformationDto> GetProjectsInformationAsync(FiltersDtoIn filters, int projectsPackageNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<FiltersDtoOut> GetFiltersAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<GeneralInformationDto> GetGeneralInformationAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <returns>  </returns>
        public Task<ProjectDto> GetProjectInformationAsync(int projectId);
    }
}