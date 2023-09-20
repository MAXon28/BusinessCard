using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("TasksCounters")]
    [NeedInsertId]
    public class TaskCounter
    {
        /// <summary>
        /// 
        /// </summary>
        public int Counter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Tasks")]
        public int TaskId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("CalculatedValues")]
        public int CalculatedValueId { get; set; }
    }
}