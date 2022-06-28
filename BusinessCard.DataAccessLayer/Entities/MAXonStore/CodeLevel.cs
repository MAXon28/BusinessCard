using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("CodeLevels")]
    public class CodeLevel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Percentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Annotation { get; set; }
    }
}