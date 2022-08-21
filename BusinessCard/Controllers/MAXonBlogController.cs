using BusinessCard.BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MAXonBlogController : Controller
    {
        private readonly IBlogService _blogService;

        public MAXonBlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult Catalog() => View();

        [HttpGet]
        public async Task<string> GetBlogInformation() => JsonConvert.SerializeObject(new
        {
            BlogInformation = await _blogService.GetBlogInformationAsync(null),
            AuthorizedUser = true
        });
    }
}