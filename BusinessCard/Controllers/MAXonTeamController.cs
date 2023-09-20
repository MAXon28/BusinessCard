using BusinessCard.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(Roles = $"{Roles.MAXon28}, {Roles.Admin}")]
    public class MAXonTeamController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult Management() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = Roles.MAXon28)]
        public IActionResult MAXon28Profile() => View();
    }
}