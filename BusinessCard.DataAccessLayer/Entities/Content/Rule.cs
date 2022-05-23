using BusinessCard.DataAccessLayer.Entities.Content.Services;
using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.Content
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Rules")]
    public class Rule
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }

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