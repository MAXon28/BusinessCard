using BusinessCard.DataAccessLayer.Entities.Data;
using DapperAssistant;
using DapperAssistant.Annotations;
using System;

namespace BusinessCard.DataAccessLayer.Entities.MAXonStore
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("ProjectReviews")]
    public class ProjectReview
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Projects")]
        public int ProjectId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Users", TypeOfJoin.LEFT)]
        public int? UserId { get; set; }
#nullable enable
        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public User? User { get; set; }
#nullable disable
    }
}