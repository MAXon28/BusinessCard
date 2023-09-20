using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Entities.DTO.Store;
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

        /// <summary>
        /// 
        /// </summary>
        private readonly IProjectReviewService _projectReviewService;

        public MAXonStoreController(IStoreService storeService, IProjectReviewService projectReviewService)
        {
            _storeService = storeService;
            _projectReviewService = projectReviewService;
        }

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
        public async Task<JsonResult> GetAllProjectsData([FromQuery] FiltersIn filtersDtoIn)
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
        public async Task<JsonResult> GetProjects([FromQuery] FiltersIn filtersDtoIn, [FromQuery] int pageNumber, [FromQuery] bool needUpdatePagesCount)
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
        [HttpGet]
        public IActionResult Project(int projectId) => View();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"> </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetProject(int projectId) => Json(await _storeService.GetProjectInformationAsync(projectId));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <param name="pageNumber">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetReviews([FromQuery] int projectId, [FromQuery] int pageNumber) => Json(await _projectReviewService.GetReviewsAsync(projectId, pageNumber));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetReviewsStatistic([FromQuery] int projectId) => Json(await _projectReviewService.GetReviewInformationAsync(projectId, false));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <param name="userName">  </param>
        /// <param name="rating">  </param>
        /// <param name="reviewText">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<JsonResult> CreateReview(int projectId, string userName, int rating, string reviewText) => Json(await _projectReviewService.CreateReview(projectId, userName, rating, reviewText));

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        //[HttpGet]
        //public async Task<JsonResult> GetGeneralInformation() => Json(await _storeService.GetDownloadsCountAsync());
    }
}