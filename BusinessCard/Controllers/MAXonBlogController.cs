using Microsoft.AspNetCore.Mvc;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MAXonBlogController : Controller
    {
        public MAXonBlogController()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult Catalog() => View();

        [HttpGet]
        [Route("{id:maxlength(100)}")]
        public JsonResult GetProjects([FromQuery] string id) => Json(true); 
    }
}