using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPostRepository : IRepository<Post> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset">  </param>
        /// <returns>  </returns>
        public Task<IEnumerable<Post>> GetPostsAsync(int offset);
    }
}