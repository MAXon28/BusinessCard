using BusinessCard.BusinessLogicLayer.Utils;

namespace BusinessCard.BusinessLogicLayer.DTOs.Blog
{
    /// <summary>
    /// 
    /// </summary>
    public class PostFilters
    {
        /// <summary>
        /// 
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PostsPackageNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SearchText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeOfRequest { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool NeedPagesCount { get; set; }
    }
}