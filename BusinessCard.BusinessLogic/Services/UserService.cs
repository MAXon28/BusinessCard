using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Interfaces.Utils;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Exceptions;
using BusinessCard.Crypto;
using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using BusinessCard.Entities.DTO;
using DapperAssistant;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IUserService"/>
    internal class UserService : IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        private const int DefaultRole = 3;

        /// <summary>
        /// 
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IValidator _validator;

        public UserService(IUserRepository userRepository, IValidator validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public async Task<int> CreateNewUserAsync(UserIn user)
        {
            var email = _validator.ValidateEmail(user.Email)
                        ? user.Email
                        : throw new MAXonValidationException("Неправильный формат email.", ValidationTypes.Email);

            var password = _validator.ValidatePassword(user.Password)
                            ? Cryptographer.EncryptPassword(user.Password)
                            : throw new MAXonValidationException("Пароль должен содержать от 8 до 26 символов, включая большие и заглавные латинские буквы, цифры и специальные символы (.@_%+-/|).", ValidationTypes.Password);

            var phoneNumber = _validator.ValidatePhoneNumber(user.PhoneNumber)
                                ? user.PhoneNumber
                                : throw new MAXonValidationException("Неправильный формат номера телефона.", ValidationTypes.PhoneNumber);

            try
            {
                return await _userRepository.AddAsync<int>(new User
                {
                    Surname = user.Surname,
                    Name = user.Name,
                    MiddleName = user.MiddleName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Password = password,
                    RoleId = DefaultRole
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cannot insert duplicate key"))
                    throw new MAXonValidationException("Пользователь с таким email уже зарегистрирован.", ValidationTypes.UniqueValue);
                throw new Exception(ex.ToString());
            }
        }

        public async Task<(bool IsIdentified, UserCookieData UserData)> IdentifyUserAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            if (user is not null && Cryptographer.EncryptPassword(password) == user.Password)
                return (IsIdentified: true, UserData: new UserCookieData
                {
                    Id = user.Id,
                    Name = user.Name,
                    Role = user.Role.Name
                });

            return (IsIdentified: false, UserData: null);
        }

        public async Task<UserOut> GetSmallUserDataAsync(int userId)
        {
            var data = await _userRepository.GetSmallUserDataAsync(userId);
            return new()
            {
                Id = data.Id,
                Surname = data.Surname,
                Name = data.Name,
                UserAbbreviation = GetUserAbbreviation(data.Surname, data.Name),
                RoleName = data.RoleName
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surname">  </param>
        /// <param name="name">  </param>
        /// <returns>  </returns>
        private string GetUserAbbreviation(string surname, string name)
            => surname.Remove(1) + name.Remove(1);

        public async Task<UserOut> GetUserInfoAsync(int userId)
        {
            var data = await GetUserByIdAsync(userId);
            return new()
            {
                Surname = data.Surname,
                Name = data.Name,
                MiddleName = data.MiddleName,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber
            };
        }

        public async Task<bool> UpdateUserProfileAsync(int userId, UserIn user)
        {
            var newEmail = _validator.ValidateEmail(user.Email)
                    ? user.Email
                    : throw new MAXonValidationException("Неправильный формат email.", ValidationTypes.Email);

            var phoneNumber = _validator.ValidatePhoneNumber(user.PhoneNumber)
                                ? user.PhoneNumber
                                : throw new MAXonValidationException("Неправильный формат номера телефона.", ValidationTypes.PhoneNumber);

            return await _userRepository.UpdateUserProfileAsync(userId, user.Surname, user.Name, user.MiddleName, newEmail, phoneNumber);
        }

        public async Task<bool> UpdateUserPasswordAsync(int userId, string currenctPassword, string newPassword)
        {
            var newEncryptPassword = _validator.ValidatePassword(newPassword)
                           ? Cryptographer.EncryptPassword(newPassword)
                           : throw new MAXonValidationException("Пароль должен содержать от 8 до 26 символов, включая большие и заглавные латинские буквы, цифры и специальные символы (.@_%+-/|).", ValidationTypes.Password);

            var currentEncryptPassword = Cryptographer.EncryptPassword(currenctPassword);

            return await _userRepository.UpdateUserPasswordAsync(userId, currentEncryptPassword, newEncryptPassword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        private async Task<User> GetUserByIdAsync(int userId)
        {
            var querySettings = new QuerySettings
            {
                ConditionField = "Id",
                ConditionType = ConditionType.EQUALLY,
                ConditionFieldValue = userId
            };
            return (await _userRepository.GetWithConditionAsync(querySettings)).First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email">  </param>
        /// <returns>  </returns>
        private async Task<User> GetUserByEmailAsync(string email)
        {
            var querySettings = new QuerySettings
            {
                ConditionField = "Email",
                ConditionType = ConditionType.EQUALLY,
                ConditionFieldValue = email
            };
            return (await _userRepository.GetWithConditionAsync(querySettings)).First();
        }
    }
}