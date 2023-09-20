namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ExtendedTaskData : TaskDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RateName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SuggestedPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Deadline { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TechnicalSpecification { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TechnicalSpecificationFilePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TechnicalSpecificationFileNameForUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DoneTaskFilePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DoneTaskFileNameForUser { get; set; }

        /// <summary>
        /// Идентификатор услуги, по которой создана задача
        /// </summary>
        public int ServiceId { get; set; }
    }
}