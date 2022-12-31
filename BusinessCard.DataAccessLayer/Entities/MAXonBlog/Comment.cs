using BusinessCard.DataAccessLayer.Entities.Data;
using DapperAssistant.Annotations;
using System;

namespace BusinessCard.DataAccessLayer.Entities.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Comments")]
    public class Comment
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Users")]
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("CommentBranches")]
        public int BranchId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Comments")]
        public int? CommentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [RelatedSqlEntity]
        public User User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotSqlColumn]
        public string UserName { get; set; }
    }
}