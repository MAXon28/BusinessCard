namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class NewTask
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserSurname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserMiddleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserPhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SuggestedPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TechnicalSpecification { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TechnicalSpecificationFileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? RateId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ServiceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? UserId { get; set; }
    }
}