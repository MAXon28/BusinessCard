using BusinessCard.DataAccessLayer.Entities.Data;
using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("ServicesCalculatedValues")]
    public class ServiceCalculatedValue
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("CalculatedValues")]
        public int CalculatedValueId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public CalculatedValue CalculatedValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ServiceId { get; set; }
    }
}