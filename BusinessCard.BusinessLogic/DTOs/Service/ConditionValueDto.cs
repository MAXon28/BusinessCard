namespace BusinessCard.BusinessLogicLayer.DTOs.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ConditionValueDto
    {
        public ConditionValueDto(bool isAvailable, string value = null)
        {
            IsAvailable = isAvailable;
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAvailable { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }
    }
}