using BusinessCard.BusinessLogicLayer.Interfaces.Utils;
using System;
using System.Text.RegularExpressions;

namespace BusinessCard.BusinessLogicLayer.Utils
{
    /// <inheritdoc cref="IValidator"/>
    public class Validator : IValidator
    {
        /// <inheritdoc/>
        public bool ValidateEmail(string email)
        {
            const string regularPattern = "^[a-zA-Z0-9._%+-]+@[a-z0-9-]+[.]{1}[a-z]{2,4}$";
            return Regex.IsMatch(email, regularPattern);
        }

        /// <inheritdoc/>
        public bool ValidatePassword(string password)
        {
            const string regularPattern = @"^[a-zA-Z0-9.@_%+-/\]{8,26}$";
            return Regex.IsMatch(password, regularPattern);
        }

        /// <inheritdoc/>
        public bool ValidatePhoneNumber(string phoneNumber)
        {
            const string regularPattern = @"^{+}?[0-9]{11}$";
            return Regex.IsMatch(phoneNumber, regularPattern);
        }
    }
}