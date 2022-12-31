using BusinessCard.BusinessLogicLayer.DTOs.Blog;
using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChannelService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<List<ChannelDto>> GetChannelsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        public Task<List<ChannelDto>> GetLimitedNumberChannelsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId">  </param>
        /// <returns>  </returns>
        public Task<(ChannelDto Channel, List<PostDto> Posts)> GetChannelDataWithPostsAsync(int channelId);
    }
}