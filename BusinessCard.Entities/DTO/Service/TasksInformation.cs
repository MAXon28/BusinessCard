namespace BusinessCard.Entities.DTO.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TasksInformation
    {
        /// <summary>
        /// 
        /// </summary>
        public int TasksCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TasksPackageCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<TaskDto> Tasks { get; set; }
    }
}