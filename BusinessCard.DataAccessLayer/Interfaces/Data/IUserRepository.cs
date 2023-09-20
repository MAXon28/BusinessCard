using BusinessCard.DataAccessLayer.Entities.Data;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserRepository : IRepository<User> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<User> GetSmallUserDataAsync(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="surname">  </param>
        /// <param name="name">  </param>
        /// <param name="middleName">  </param>
        /// <param name="newEmail">  </param>
        /// <param name="newPhoneNumber">  </param>
        /// <returns>  </returns>
        public Task<bool> UpdateUserProfileAsync(int userId, string surname, string name, string middleName, string newEmail, string newPhoneNumber);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="currentPassword">  </param>
        /// <param name="newPassword">  </param>
        /// <returns>  </returns>
        public Task<bool> UpdateUserPasswordAsync(int userId, string currentPassword, string newPassword);
    }
}