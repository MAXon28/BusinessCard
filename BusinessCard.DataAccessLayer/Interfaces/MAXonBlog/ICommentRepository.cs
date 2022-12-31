using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommentRepository : IRepository<Comment> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <returns>  </returns>
        public Task<IEnumerable<Comment>> GetAllCommentsAsync(int postId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="branchId">  </param>
        /// <returns>  </returns>
        public Task<IEnumerable<Comment>> GetCommentsAsync(int postId, int branchId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="branchId">  </param>
        /// <returns></returns>
        public Task<IEnumerable<Comment>> GetAllNextCommentsAsync(int postId, int branchId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="branchId">  </param>
        /// <param name="lastCommentId">  </param>
        /// <returns>  </returns>
        public Task<IEnumerable<Comment>> GetCommentsByBranchAsync(int postId, int branchId, int lastCommentId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId">  </param>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<int> MarkCommentAsDeletedAsync(int commentId, int userId);
    }
}