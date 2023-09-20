using BusinessCard.Entities.DTO;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// Сервис пользователя
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <param name="user">  </param>
        /// <returns> Идентификатор пользователя </returns>
        public Task<int> CreateNewUserAsync(UserIn user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">  </param>
        /// <param name="password">  </param>
        /// <returns>  </returns>
        public Task<(bool IsIdentified, UserCookieData UserData)> IdentifyUserAsync(string email, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<UserOut> GetSmallUserDataAsync(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<UserOut> GetUserInfoAsync(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="user">  </param>
        /// <returns>  </returns>
        public Task<bool> UpdateUserProfileAsync(int userId, UserIn user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="currenctPassword">  </param>
        /// <param name="newPassword">  </param>
        /// <returns>  </returns>
        public Task<bool> UpdateUserPasswordAsync(int userId, string currenctPassword, string newPassword);
    }
}