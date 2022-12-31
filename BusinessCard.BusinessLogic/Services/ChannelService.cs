using BusinessCard.BusinessLogicLayer.DTOs.Blog;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ChannelService : IChannelService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IChannelRepository _channelRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IPostService _postService;

        public ChannelService(IChannelRepository channelRepository, IPostService postService)
        {
            _channelRepository = channelRepository;
            _postService = postService;
        }

        public async Task<List<ChannelDto>> GetChannelsAsync()
            => (await _channelRepository.GetAsync(needSortDescendingOrder: true)).Select(channel => GetChannelInDto(channel)).ToList();

        public async Task<List<ChannelDto>> GetLimitedNumberChannelsAsync()
        {
            const int countLimit = 5;

            var channels = await _channelRepository.GetAsync(countLimit, true);

            var result = new List<ChannelDto>();

            foreach (var channel in channels)
                result.Add(GetChannelInDto(channel));

            return result;
        }

        public async Task<(ChannelDto Channel, List<PostDto> Posts)> GetChannelDataWithPostsAsync(int channelId)
        {
            var data = await _channelRepository.GetChannelDataAsync(channelId);
            return (GetChannelInDto(data), _postService.GetPostsInDto(data.Posts));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channel">  </param>
        /// <returns>  </returns>
        private ChannelDto GetChannelInDto(Channel channel)
            => new ChannelDto
            {
                Id = channel.Id,
                Name = channel.Name,
                Color = channel.Color,
                Description = channel.Description
            };
    }
}