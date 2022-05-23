using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.Content
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Skills")]
    public class Skill
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
        public int PercentOfKnowledge { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}