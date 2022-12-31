using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.DTOs.Blog
{
    /// <summary>
    /// 
    /// </summary>
    public class PostsInformation
    {
        /// <summary>
        /// 
        /// </summary>
        public List<PostDto> Posts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PersonalInformation PersonalInformation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PagesCount { get; set; }
    }
}