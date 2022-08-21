using BusinessCard.BusinessLogicLayer.DTOs.Blog;
using BusinessCard.BusinessLogicLayer.Interfaces;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class BlogService : IBlogService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IChannelService _channelService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPostService _postService;

        public BlogService(IChannelService channelService, IPostService postService)
        {
            _channelService = channelService;
            _postService = postService;
        }

        public async Task<BlogIngormation> GetBlogInformationAsync(int? userId)
            => new BlogIngormation
            {
                Channels = await _channelService.GetLimitedNumberChannelsAsync(),
                Posts = await _postService.GetPostsAsync(1)
            };
    }
}