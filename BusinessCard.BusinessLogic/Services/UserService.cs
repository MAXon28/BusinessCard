using BusinessCard.BusinessLogicLayer.DTOs;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<bool> CreateNewUserAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<UserCookieData> IdentifyUserAsync(string email, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}