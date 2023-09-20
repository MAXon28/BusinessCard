using BusinessCard.BusinessLogicLayer.Utils.Enums;
using System;

namespace BusinessCard.BusinessLogicLayer.Utils.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class MAXonValidationException : Exception
    {
        public MAXonValidationException() { }

        public MAXonValidationException(string message) : base(message) { }

        public MAXonValidationException(string message, ValidationTypes validationType) : base(message) => ValidationType = validationType;

        /// <summary>
        /// 
        /// </summary>
        public ValidationTypes ValidationType { get; }
    }
}