using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("TaskStatuses")]
    public class TaskStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int StatusCode { get; set; }
    }
}