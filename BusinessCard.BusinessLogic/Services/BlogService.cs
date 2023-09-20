using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.Entities.DTO.Blog;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <summary>
    /// 
    /// </summary>
    internal class BlogService : IBlogService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IChannelService _channelService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPostService _postService;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPersonalInformationService _personalInformationService;

        public BlogService(IChannelService channelService, IPostService postService, IPersonalInformationService personalInformationService)
        {
            _channelService = channelService;
            _postService = postService;
            _personalInformationService = personalInformationService;
        }

        public async Task<BlogInformation> GetBlogInformationAsync(int userId)
        {
            var information = await _postService.GetFirstBlockOfPostsAsync(userId != -1 ? userId : null);
            information.Channels = await _channelService.GetLimitedNumberChannelsAsync();
            return information;
        }

        public async Task<ChannelInformation> GetChannelInformationAsync(int userId, int channelId)
        {
            var information = new ChannelInformation();
            var data = await _channelService.GetChannelDataWithPostsAsync(channelId);
            information.Channel = data.Channel;
            information.Posts = data.Posts;
            information.PersonalInformation = userId != -1
                ? await _personalInformationService.GetPersonalInformationAsync(userId, channelId, _postService.GetPostsId(information.Posts))
                : null;
            information.PagesCount = await _postService.GetPagesCountByChannelAsync(channelId);
            return information;
        }
    }
}