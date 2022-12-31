using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.Data
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Roles")]
    public class Role
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