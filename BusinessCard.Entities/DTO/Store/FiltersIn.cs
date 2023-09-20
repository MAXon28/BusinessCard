namespace BusinessCard.Entities.DTO.Store
{
    /// <summary>
    /// 
    /// </summary>
    public class FiltersIn
    {
        /// <summary>
        /// 
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int LastProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int SortType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<int> ProjectTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<int> ProjectCategories { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<int> ProjectCompatibilities { get; set; }
    }
}