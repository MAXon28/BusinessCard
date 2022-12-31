using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserStatisticRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="postsId">  </param>
        /// <returns>  </returns>
        public Task<Dictionary<int, Dictionary<string, bool>>> GetUserStatisticByPostsAsync(int userId, IEnumerable<int> postsId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="channelId">  </param>
        /// <returns>  </returns>
        public Task<Dictionary<string, bool>> GetUserSubscriptionsByPostsAsync(int userId, int channelId);
    }
}