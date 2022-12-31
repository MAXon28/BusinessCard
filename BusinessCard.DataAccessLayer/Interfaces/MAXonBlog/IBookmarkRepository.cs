using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBookmarkRepository : IRepository<Bookmark> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">  </param>
        /// <param name="postId">  </param>
        /// <returns>  </returns>
        public Task<int> DeletePostFromBookmarkAsync(int userId, int postId);
    }
}