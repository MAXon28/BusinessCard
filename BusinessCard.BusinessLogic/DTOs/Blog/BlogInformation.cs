using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.DTOs.Blog
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