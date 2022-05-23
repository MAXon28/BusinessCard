using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("ProjectCompatibilities")]
    public class ProjectCompatibility
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("CompatibilitySections")]
        public int CompatibilitySectionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public CompatibilitySection CompatibilitySection { get; set; }
    }
}