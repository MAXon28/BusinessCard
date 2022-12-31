using BusinessCard.BusinessLogicLayer.DTOs.Blog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPersonalInformationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="postsId">  </param>
        /// <returns>  </returns>
        public Task<PersonalInformation> GetPersonalInformationAsync(int userId, IEnumerable<int> postsId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="channelId">  </param>
        /// <param name="postsId">  </param>
        /// <returns>  </returns>
        public Task<PersonalInformation> GetPersonalInformationAsync(int userId, int channelId, IEnumerable<int> postsId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<Dictionary<int, ChannelDto>> GetSubscriptionOnChannelsAsync(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<Dictionary<int, ChannelDto>> GetSubscriptionOnMailingsAsync(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="postId">  </param>
        /// <param name="isTopchik">  </param>
        /// <returns>  </returns>
        public Task<bool> AddOrDeleteTopchikAsync(int userId, int postId, bool isTopchik);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="postId">  </param>
        /// <returns>  </returns>
        public Task<bool> AddOrDeletePostInBookmarkAsync(int userId, int postId, bool inBookmark);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="channelId">  </param>
        /// <param name="isSubscribe">  </param>
        /// <param name="subscribeTypeStr">  </param>
        /// <returns>  </returns>
        public Task<bool> SubscribeAsync(int userId, int channelId, bool isSubscribe, string subscribeTypeStr);
    }
}