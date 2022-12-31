using BusinessCard.BusinessLogicLayer.DTOs.Blog;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBlogService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<BlogInformation> GetBlogInformationAsync(int? userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="channelId">  </param>
        /// <returns>  </returns>
        public Task<ChannelInformation> GetChannelInformationAsync(int? userId, int channelId);
    }
}