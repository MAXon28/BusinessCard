using BusinessCard.BusinessLogicLayer.DTOs;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<bool> CreateNewUserAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">  </param>
        /// <param name="password">  </param>
        /// <returns>  </returns>
        public Task<UserCookieData> IdentifyUserAsync(string email, string password);
    }
}