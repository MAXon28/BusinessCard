using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Conditions")]
    public class Condition
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConditionText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Services")]
        public int ServiceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public Service Service { get; set; }
    }
}