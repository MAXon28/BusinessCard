namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class AdvancedServiceDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int ServiceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DefaultPrice { get; set; }

        /// <summary>
        /// +
        /// </summary>
        public bool NeedTechnicalSpecification { get; set; }

        /// <summary>
        /// +
        /// </summary>
        public bool PrePrice { get; set; }

        /// <summary>
        /// +
        /// </summary>
        public bool PreDeadline { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<UserVersionRate> Rates { get; set; }
    }
}