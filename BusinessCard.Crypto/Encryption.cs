using System;
using System.Security.Cryptography;
using System.Text;

namespace BusinessCard.Crypto
{
    /// <summary>
    /// 
    /// </summary>
    public static class Encryption
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password">  </param>
        /// <returns>  </returns>
        public static string EncryptPassword(string password)
        {
            using var sha = SHA512.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }
}
