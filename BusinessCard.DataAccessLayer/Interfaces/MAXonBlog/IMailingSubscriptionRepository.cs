using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMailingSubscriptionRepository : IRepository<MailingSubscription> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="channelId">  </param>
        /// <returns>  </returns>
        public Task<int> DeleteChannelFromSubscriptionAsync(int userId, int channelId);
    }
}