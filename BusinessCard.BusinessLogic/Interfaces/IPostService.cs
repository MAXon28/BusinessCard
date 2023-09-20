using BusinessCard.Entities.DTO.Blog;
using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="filters">  </param>
        /// <returns>  </returns>
        public Task<PostsInformation> GetPostsAsync(int userId, PostFilters filters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<BlogInformation> GetFirstBlockOfPostsAsync(int? userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId">  </param>
        /// <returns>  </returns>
        public Task<int> GetPagesCountByChannelAsync(int channelId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="posts">  </param>
        /// <returns>  </returns>
        public List<PostDto> GetPostsInDto(IEnumerable<Post> posts);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="posts">  </param>
        /// <returns>  </returns>
        public IEnumerable<int> GetPostsId(IEnumerable<PostDto> posts);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="postKey">  </param>
        /// <returns>  </returns>
        public Task<PostInformation> GetPostInformationAsync(int userId, string postKey);
    }
}