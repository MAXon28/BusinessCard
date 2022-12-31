using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Interfaces.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommentBranchRepository : IRepository<CommentBranch> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="branchesId">  </param>
        /// <returns>  </returns>
        public Task<Dictionary<int, int>> GetCommentsCountInBranchesAsync(IEnumerable<int> branchesId);
    }
}