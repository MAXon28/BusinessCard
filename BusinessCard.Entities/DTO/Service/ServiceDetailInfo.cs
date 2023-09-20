namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceDetailInfo : ServiceDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ConcretePrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Price { get; set; }

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
        public bool IsPublic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<ShortDescriptionData> ShortDescriptions { get; set; }
    }
}