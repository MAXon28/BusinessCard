namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskFilters
    {
        /// <summary>
        /// 
        /// </summary>
        public int LastTaskId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeOfStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool NeedPackagesCount { get; set; }
    }
}