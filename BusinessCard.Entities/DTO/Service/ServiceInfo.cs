namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceInfo : ServiceDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<string> ShortDescriptions { get; set; }
    }
}