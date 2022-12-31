namespace BusinessCard.BusinessLogicLayer.DTOs.Blog
{
    /// <summary>
    /// 
    /// </summary>
    public class CommentIn
    {
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BranchId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? CommentId { get; set; }
    }
}