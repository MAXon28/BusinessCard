using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">  </param>
        /// <returns>  </returns>
        public bool ValidateEmail(string email);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password">  </param>
        /// <returns>  </returns>
        public bool ValidatePassword(string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneNumber">  </param>
        /// <returns>  </returns>
        public bool ValidatePhoneNumber(string phoneNumber);
    }
}