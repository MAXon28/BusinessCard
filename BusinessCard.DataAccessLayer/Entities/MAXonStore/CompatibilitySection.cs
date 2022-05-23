using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("CompatibilitySections")]
    public class CompatibilitySection
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SectionName { get; set; }
    }
}