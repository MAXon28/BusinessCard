using System.Collections.Generic;

namespace BusinessCard.BusinessLogicLayer.DTOs.Blog
{
    /// <summary>
    /// 
    /// </summary>
    public class PostInformation
    {
        /// <summary>
        /// 
        /// </summary>
        public PostDto Post { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PostDetail> PostDetails { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PersonalInformation PersonalInformation { get; set; }
    }
}