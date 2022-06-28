using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("ProjectTechnicalRequirementValues")]
    public class ProjectTechnicalRequirementValue
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("ProjectTechnicalRequirementNames")]
        public int NameId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public ProjectTechnicalRequirementName ProjectTechnicalRequirementName { get; set; }
    }
}