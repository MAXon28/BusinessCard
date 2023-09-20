namespace BusinessCard.Entities.DTO.Blog
{
    /// <summary>
    /// 
    /// </summary>
    public class BlogInformation : PostsInformation
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ChannelDto> Channels { get; set; }
    }
}