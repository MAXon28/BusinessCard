using BusinessCard.BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MAXonBusinessCardController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IBusinessCardService _businessCardService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IAboutMeService _aboutMeService;

        public MAXonBusinessCardController(
            IBusinessCardService businessCardService, 
            IAboutMeService aboutMeService)
        {
            _businessCardService = businessCardService;
            _aboutMeService = aboutMeService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult Card() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetMainFacts() => Json(await _businessCardService.GetFactsAsync());

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult AboutMe() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<JsonResult> GetAboutMeData() => Json(await _aboutMeService.GetInformationAboutMe());
    }
}