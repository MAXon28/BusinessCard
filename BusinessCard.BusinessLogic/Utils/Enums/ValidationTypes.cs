using BusinessCard.BusinessLogicLayer.Utils.Attributes;

namespace BusinessCard.BusinessLogicLayer.Utils.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum ValidationTypes
    {
        /// <summary>
        /// 
        /// </summary>
        [MAXon28("unique_Value")]
        UniqueValue,

        /// <summary>
        /// 
        /// </summary>
        [MAXon28("email")]
        Email,

        /// <summary>
        /// 
        /// </summary>
        [MAXon28("password")]
        Password,

        /// <summary>
        /// 
        /// </summary>
        [MAXon28("phone")]
        PhoneNumber
    }
}