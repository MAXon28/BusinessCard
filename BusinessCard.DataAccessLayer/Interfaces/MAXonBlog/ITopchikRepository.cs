using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITopchikRepository : IRepository<Topchik> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="postId">  </param>
        /// <returns>  </returns>
        public Task<int> DeleteTopchikAsync(int userId, int postId);
    }
}