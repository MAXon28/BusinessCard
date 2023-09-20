using BusinessCard.DataAccessLayer.Entities.Data;
using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonService
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("TasksPersonalInformation")]
    public class TaskPersonalInfo
    {
        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Users")]
        public int? UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserSurname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserMiddleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserPhoneNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Tasks")]
        public int TaskId { get; set; }

#nullable enable
        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public User? User { get; set; }
    }
}