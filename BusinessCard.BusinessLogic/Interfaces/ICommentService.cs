using BusinessCard.Entities.DTO.Blog;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="commentsCount">  </param>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<CommentsInformation> GetFirstCommentsInformationAsync(int postId, int commentsCount, int? userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="lastBranchId">  </param>
        /// <param name="allNextComments">  </param>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<CommentsInformation> GetCommentsInformationAsync(int postId, int lastBranchId, bool allNextComments, int? userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="branchId">  </param>
        /// <param name="lastCommentId">  </param>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<IEnumerable<CommentOut>> GetCommentsByBranchAsync(int postId, int branchId, int lastCommentId, int? userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment">  </param>
        /// <returns>  </returns>
        public Task<(int BranchId, int CommentId)> CreateCommentAsync(CommentIn comment);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId">  </param>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        public Task<bool> DeleteCommentAsync(int commentId, int userId);
    }
}