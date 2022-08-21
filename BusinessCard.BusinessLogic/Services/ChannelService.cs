using BusinessCard.BusinessLogicLayer.DTOs.Blog;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using System.Collections.Generic;
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

        public ChannelService(IChannelRepository channelRepository) => _channelRepository = channelRepository;

        public async Task<List<ChannelDto>> GetLimitedNumberChannelsAsync()
        {
            const int countLimit = 5;

            var channels = await _channelRepository.GetAsync(countLimit, true);

            var result = new List<ChannelDto>();

            foreach (var channel in channels)
                result.Add(new ChannelDto
                {
                    Id = channel.Id,
                    Name = channel.Name,
                    Color = channel.Color,
                    Description = channel.Description
                });

            return result;
        }
    }
}