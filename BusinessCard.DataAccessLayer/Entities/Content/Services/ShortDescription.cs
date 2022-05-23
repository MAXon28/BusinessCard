using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("ServiceShortDescriptions")]
    public class ShortDescription
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Data { get; set; }

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