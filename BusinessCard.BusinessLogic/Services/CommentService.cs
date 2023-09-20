using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using BusinessCard.Entities.DTO.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <summary>
    /// 
    /// </summary>
    internal class CommentService : ICommentService
    {
        /// <summary>
        /// Максимальное допустимое количество комментариев в одном пакете для отправки клиенту
        /// </summary>
        private const int MaxCommentsCountInOnePackage = 56;

        /// <summary>
        /// 
        /// </summary>
        private readonly ICommentBranchRepository _commentBranchRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentBranchRepository commentBranchRepository, ICommentRepository commentRepository)
        {
            _commentBranchRepository = commentBranchRepository;
            _commentRepository = commentRepository;
        }

        public async Task<CommentsInformation> GetFirstCommentsInformationAsync(int postId, int commentsCount, int? userId)
            => (commentsCount <= MaxCommentsCountInOnePackage) switch
            {
                true => await GetAllCommentsInformationAsync(postId, userId),
                false => await GetCommentsInformationAsync(postId, 0, false, userId)
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId">  </param>
        /// <param name="userId">  </param>
        /// <returns>  </returns>
        private async Task<CommentsInformation> GetAllCommentsInformationAsync(int postId, int? userId)
        {
            var comments = (await _commentRepository.GetAllCommentsAsync(postId)).ToArray();
            return await BuildCommentsInformationAsync(comments, userId);
        }

        public async Task<CommentsInformation> GetCommentsInformationAsync(int postId, int lastBranchId, bool allNextComments, int? userId)
        {
            var comments = allNextComments
                    ? (await _commentRepository.GetAllNextCommentsAsync(postId, lastBranchId)).ToArray()
                    : (await _commentRepository.GetCommentsAsync(postId, lastBranchId)).ToArray();
            return await BuildCommentsInformationAsync(comments, userId);
        }

        /// <summary>
        /// Сборка информации о комментариях
        /// </summary>
        /// <param name="comments"> Список комментариев </param>
        /// <param name="userId"> Идентификатор пользователя, который сделал запрос (если он авторизован) </param>
        /// <returns> Информация о комментарии </returns>
        private async Task<CommentsInformation> BuildCommentsInformationAsync(IEnumerable<Comment> comments, int? userId)
        {
            var branchesId = comments.Select(comment => comment.BranchId).Distinct();
            var commentsCountInBranches = await _commentBranchRepository.GetCommentsCountInBranchesAsync(comments.Select(comment => comment.BranchId).Distinct());
            return new()
            {
                Branches = comments
                    .GroupBy(comment => comment.BranchId)
                    .OrderBy(x => x.Key)
                    .Select(x =>
                        new Branch
                        {
                            Id = x.Key,
                            Comments = x.Select(v => new CommentOut
                            {
                                Id = v.Id,
                                Text = v.IsDeleted is false ? v.Text : string.Empty,
                                Time = v.Time.ConvertToReadableFormatWithTime(),
                                IsDeleted = v.IsDeleted,
                                UserName = v.IsDeleted is false ? v.UserName : string.Empty,
                                CommentId = v.CommentId,
                                BelongsUser = v.IsDeleted is false && userId is not null && v.UserId == userId
                            }).ToList(),
                            CommentsCount = commentsCountInBranches[x.Key]
                        })
                    .ToList()
            };
        }

        public async Task<IEnumerable<CommentOut>> GetCommentsByBranchAsync(int postId, int branchId, int lastCommentId, int? userId) =>
            (await _commentRepository.GetCommentsByBranchAsync(postId, branchId, lastCommentId))
                .Select(x => new CommentOut
                {
                    Id = x.Id,
                    Text = !x.IsDeleted ? x.Text : string.Empty,
                    Time = x.Time.ConvertToReadableFormatWithTime(),
                    IsDeleted = x.IsDeleted,
                    UserName = !x.IsDeleted ? x.UserName : string.Empty,
                    CommentId = x.CommentId,
                    BelongsUser = !x.IsDeleted && !(userId is null) && x.UserId == userId
                });


        public async Task<(int BranchId, int CommentId)> CreateCommentAsync(CommentIn comment)
        {
            var commentBranchId = comment.BranchId;
            if (commentBranchId == 0)
                commentBranchId = await _commentBranchRepository.AddAsync<int>(new CommentBranch { PostId = comment.PostId });
            var newCommentId = await _commentRepository.AddAsync<int>(new Comment
            {
                Text = comment.Text,
                Time = DateTime.Now,
                IsDeleted = false,
                UserId = comment.UserId,
                BranchId = commentBranchId,
                CommentId = comment.CommentId > 0 ? comment.CommentId : null
            });
            return (commentBranchId, newCommentId);
        }

        public async Task<bool> DeleteCommentAsync(int commentId, int userId) =>
            await _commentRepository.MarkCommentAsDeletedAsync(commentId, userId) == 1;
    }
}