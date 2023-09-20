namespace BusinessCard.Entities.DTO.Blog
{
    /// <summary>
    /// 
    /// </summary>
    public class PostDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HeaderImageUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TopchiksCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int BookmarksCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ViewsCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CommentsCount { get; set; }
    }
}