using BusinessCard.BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public Task<bool> Registration()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult Login() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">  </param>
        /// <param name="password">  </param>
        /// <returns>  </returns>
        [HttpGet]
        public async Task<bool> Authenticate([FromQuery] string email, [FromQuery] string password)
        {
            var data = await _userService.IdentifyUserAsync(email, password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize]
        public IActionResult Profile() => View();
    }
}