using BusinessCard.BusinessLogicLayer.DTOs.Blog;
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
        public Task<List<ChannelDto>> GetLimitedNumberChannelsAsync();
    }
}