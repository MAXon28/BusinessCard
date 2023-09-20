using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.Data
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Users")]
    public class User
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Roles")]
        public int RoleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public Role Role { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public string RoleName { get; set; }
    }
}