using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChannelRepository : IRepository<Channel> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">  </param>
        /// <returns>  </returns>
        public Task<Channel> GetChannelDataAsync(int id);
    }
}