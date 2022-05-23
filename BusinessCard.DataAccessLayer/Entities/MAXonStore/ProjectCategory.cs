using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("ProjectCategories")]
    public class ProjectCategory
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}