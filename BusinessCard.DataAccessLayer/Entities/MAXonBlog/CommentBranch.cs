using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("CommentBranches")]
    public class CommentBranch
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Posts")]
        public int PostId { get; set; }
    }
}