namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public ExtendedTaskData Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PersonalInfo PersonalInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<Record> Records { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? HaveCounter { get; set; }
    }
}