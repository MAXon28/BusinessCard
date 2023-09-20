namespace BusinessCard.Entities.DTO.Blog
{
    /// <summary>
    /// 
    /// </summary>
    public class Branch
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CommentOut> Comments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CommentsCount { get; set; }
    }
}