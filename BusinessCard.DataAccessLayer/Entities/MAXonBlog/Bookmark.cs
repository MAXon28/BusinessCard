using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    [SqlTable("Bookmarks")]
    public class Bookmark
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Users")]
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SqlForeignKey("Posts")]
        public int PostId { get; set; }
    }
}