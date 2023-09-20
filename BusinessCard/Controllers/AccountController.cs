using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Entities;
using BusinessCard.Entities.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessCard.Controllers
{
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public IActionResult Login() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">  </param>
        /// <returns>  </returns>
        [HttpPost]
        public async Task<bool> Registration([FromBody] UserIn user)
        {
            var userId = await _userService.CreateNewUserAsync(user);
            if (userId > 0)
                await SetCookie(GetClaims(userId, user.Name));
            return userId > 0;
        }

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
            if (data.IsIdentified)
                await SetCookie(GetClaims(data.UserData));
            return data.IsIdentified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="userName">  </param>
        /// <returns>  </returns>
        private static IEnumerable<Claim> GetClaims(int userId, string userName)
            => new List<Claim>
            {
                new Claim(CookieConstants.UserId, userId.ToString()),
                new Claim(CookieConstants.UserName, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, Roles.User)
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userData">  </param>
        /// <returns>  </returns>
        private static IEnumerable<Claim> GetClaims(UserCookieData userData)
            => new List<Claim>
            {
                new Claim(CookieConstants.UserId, userData.Id.ToString()),
                new Claim(CookieConstants.UserName, userData.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userData.Role)
            };

        /// <summary>
        /// Метод, который сохраняет в куки-файл данные авторизированного пользователя (вызывается при обновлении логина пользователя)
        /// </summary>
        /// <param name="claims"> Логин </param>
        /// <returns></returns>
        private async Task SetCookie(IEnumerable<Claim> claims)
        {
            // создаем объект ClaimsIdentity
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = false,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.MaxValue,
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                IssuedUtc = DateTimeOffset.Now,
                // The time at which the authentication ticket was issued.

                RedirectUri = "http://localhost:54946/"
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id), authProperties);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize(Roles = Roles.User)]
        public IActionResult Profile() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize]
        public async Task<string> GetSmallUserData()
        {
            var userId = Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value);
            return JsonConvert.SerializeObject(new
            {
                UserData = await _userService.GetSmallUserDataAsync(userId)
            });
        }

        [HttpGet]
        [Authorize]
        public IActionResult Detail() => View();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        [Authorize]
        public async Task<string> GetProfileData()
        {
            var userId = Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value);
            return JsonConvert.SerializeObject(new
            {
                UserData = await _userService.GetUserInfoAsync(userId)
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">  </param>
        /// <returns>  </returns>
        [HttpPost]
        [Authorize]
        public async Task<bool> UpdateProfile([FromBody] UserIn user)
            => await _userService.UpdateUserProfileAsync(Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value), user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPassword">  </param>
        /// <param name="newPassword">  </param>
        /// <returns>  </returns>
        [HttpPost]
        [Authorize]
        public async Task<bool> UpdatePassword(string currentPassword, string newPassword)
            => await _userService.UpdateUserPasswordAsync(Convert.ToInt32(User.FindFirst(CookieConstants.UserId).Value), currentPassword, newPassword);

        /// <summary>
        /// Выйти из аккаунта
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Card", "MAXonBusinessCard");
        }
    }
}