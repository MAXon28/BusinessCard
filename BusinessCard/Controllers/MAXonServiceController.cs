using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Entities;
using BusinessCard.Entities.DTO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MAXonServiceController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ISelfEmployedService _selfEmployedService;

        public MAXonServiceController(ISelfEmployedService selfEmployedService) => _selfEmployedService = selfEmployedService;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult Services() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetServices() => Json(await _selfEmployedService.GetAllPublicServicesAsync());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetFullDescription(int id) => Json(await _selfEmployedService.GetFullDescriptionAsync(id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetReviews(int id)
        {
            return Json(await _selfEmployedService.GetReviewsAsync(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult ServiceTaskRegistration(int serviceId)
        {
            ViewData["ServiceId"] = serviceId;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<string> GetServiceRates(int serviceId)
            => JsonConvert.SerializeObject(new
            {
                UserId = User.Identity.IsAuthenticated ? User.FindFirst(CookieConstants.UserId).Value : null,
                AdvancedService = await _selfEmployedService.GetAdvancedServiceAsync(serviceId)
            });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<IActionResult> ServiceRule(int serviceId) => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<JsonResult> GetAllServices() => Json(await _selfEmployedService.GetAllServicesAsync());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.MAXon28)]
        public IActionResult ServiceCreator() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> AddService(ServiceDetailInfo service) => await _selfEmployedService.AddServiceAsync(service);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.MAXon28)]
        public IActionResult ServiceUpdater(int id) => View();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<JsonResult> GetServiceInfo(int serviceId) => Json(await _selfEmployedService.GetServiceDetailInfoAsync(serviceId));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="updateType"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = Roles.MAXon28)]
        public async Task<bool> UpdateService(ServiceDetailInfo service, int updateType) => await _selfEmployedService.UpdateServiceAsync(service, updateType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.MAXon28)]
        public IActionResult RateCreatorForService(int id) => View();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.MAXon28)]
        public IActionResult RateUpdater(int id) => View();
    }
}