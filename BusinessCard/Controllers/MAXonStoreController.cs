using BusinessCard.BusinessLogicLayer.DTOs.Store;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MAXonStoreController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IStoreService _storeService;

        public MAXonStoreController(IStoreService storeService) => _storeService = storeService;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult Projects() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filtersDtoIn">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetAllProjectsData([FromQuery] FiltersDtoIn filtersDtoIn)
        {
            var projectsTask = _storeService.GetProjectsAsync(filtersDtoIn, 1);
            var filtersTask = _storeService.GetFiltersAsync();
            var generalInformationTask = _storeService.GetGeneralInformationAsync();

            return Json(new ProjectsPageViewModel
                        {
                            Projects = await projectsTask,
                            Filters = await filtersTask,
                            GeneralInformation = await generalInformationTask
                        });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filtersDtoIn">  </param>
        /// <param name="pageNumber">  </param>
        /// <param name="needUpdatePagesCount">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetProjects([FromQuery] FiltersDtoIn filtersDtoIn, [FromQuery] int pageNumber, [FromQuery] bool needUpdatePagesCount)
        {
            if (needUpdatePagesCount)
                return Json(await _storeService.GetProjectsInformationAsync(filtersDtoIn, pageNumber));

            return Json(await _storeService.GetProjectsAsync(filtersDtoIn, pageNumber));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <returns></returns>
        public IActionResult Project(int projectId) => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        //[HttpGet]
        //public async Task<JsonResult> GetGeneralInformation() => Json(await _storeService.GetDownloadsCountAsync());
    }
}