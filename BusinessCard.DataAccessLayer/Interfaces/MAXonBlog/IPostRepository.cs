using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using Dapper;
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
        /// <param name="sqlQuery">  </param>
        /// <param name="parameters">  </param>
        /// <returns>  </returns>
        public Task<IEnumerable<Post>> GetPostsAsync(string sqlQuery, DynamicParameters parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlQuery">  </param>
        /// <param name="parameters">  </param>
        /// <returns>  </returns>
        public Task<int> GetPostsCountAsync(string sqlQuery, DynamicParameters parameters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postKey">  </param>
        /// <returns>  </returns>
        public Task<Post> GetPostAsync(string postKey);
    }
}