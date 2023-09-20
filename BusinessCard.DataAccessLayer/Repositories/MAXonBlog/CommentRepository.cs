using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonBlog
{
    internal class CommentRepository : StandardRepository<Comment>, ICommentRepository
    {
        public CommentRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<IEnumerable<Comment>> GetAllCommentsAsync(int postId)
        {
            const string sqlQuery = @"SELECT comment.Id,
											 comment.Text,
											 comment.Time,
											 comment.IsDeleted,
											 comment.BranchId,
											 comment.CommentId,
											 comment.UserId,
											 person.Name
									 FROM Comments comment
										INNER JOIN CommentBranches commentBranch
										ON comment.BranchId = commentBranch.Id
										INNER JOIN Users person
										ON comment.UserId = person.Id
										LEFT JOIN Comments comment2
										ON comment.CommentId = comment2.Id
									 WHERE commentBranch.PostId = @postId AND comment.Id IN (SELECT TOP 7 comment3.Id
														 FROM Comments comment3
														 WHERE comment3.BranchId = commentBranch.Id)";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QueryAsync<Comment, string, Comment>(
                    sqlQuery,
                    (comment, userName) =>
                    {
                        comment.UserName = userName;
                        return comment;
                    },
                    new { postId },
                    splitOn: "Name");
        }

		public async Task<IEnumerable<Comment>> GetCommentsAsync(int postId, int branchId)
        {
			const string sqlQuery = @"SELECT comment.Id,
											 comment.Text,
											 comment.Time,
											 comment.IsDeleted,
											 comment.BranchId,
											 comment.CommentId,
											 comment.UserId,
											 person.Name
									 FROM Comments comment
										INNER JOIN CommentBranches commentBranch
										ON comment.BranchId = commentBranch.Id
										INNER JOIN Users person
										ON comment.UserId = person.Id
										LEFT JOIN Comments comment2
										ON comment.CommentId = comment2.Id
									 WHERE commentBranch.PostId = @postId 
											AND comment.BranchId > @branchId 
											AND comment.BranchId IN (SELECT TOP 8 commentBranch2.Id
														 FROM CommentBranches commentBranch2
														 WHERE commentBranch2.Id > @branchId
																AND commentBranch2.PostId = @postId)
											AND comment.Id IN (SELECT TOP 7 comment3.Id
														 FROM Comments comment3
														 WHERE comment3.BranchId = commentBranch.Id)";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QueryAsync<Comment, string, Comment>(
                    sqlQuery,
                    (comment, userName) =>
                    {
                        comment.UserName = userName;
                        return comment;
                    },
                    new { postId, branchId },
                    splitOn: "Name");
        }

        public async Task<IEnumerable<Comment>> GetAllNextCommentsAsync(int postId, int branchId)
        {
            const string sqlQuery = @"SELECT comment.Id,
											 comment.Text,
											 comment.Time,
											 comment.IsDeleted,
											 comment.BranchId,
											 comment.CommentId,
											 comment.UserId,
											 person.Name
									 FROM Comments comment
										INNER JOIN CommentBranches commentBranch
										ON comment.BranchId = commentBranch.Id
										INNER JOIN Users person
										ON comment.UserId = person.Id
										LEFT JOIN Comments comment2
										ON comment.CommentId = comment2.Id
									 WHERE commentBranch.PostId = @postId 
											AND comment.BranchId > @branchId
											AND comment.Id IN (SELECT TOP 7 comment3.Id
														 FROM Comments comment3
														 WHERE comment3.BranchId = commentBranch.Id)";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QueryAsync<Comment, string, Comment>(
                    sqlQuery,
                    (comment, userName) =>
                    {
                        comment.UserName = userName;
                        return comment;
                    },
                    new { postId, branchId },
                    splitOn: "Name");
        }

        public async Task<IEnumerable<Comment>> GetCommentsByBranchAsync(int postId, int branchId, int lastCommentId)
		{
            const string sqlQuery = @"SELECT comment.Id,
											 comment.Text,
											 comment.Time,
											 comment.IsDeleted,
											 comment.CommentId,
											 comment.UserId,
											 person.Name
									 FROM Comments comment
										INNER JOIN CommentBranches commentBranch
										ON comment.BranchId = commentBranch.Id
										INNER JOIN Users person
										ON comment.UserId = person.Id
										LEFT JOIN Comments comment2
										ON comment.CommentId = comment2.Id
									 WHERE commentBranch.PostId = @postId 
											AND comment.BranchId = @branchId 
											AND comment.Id > @lastCommentId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QueryAsync<Comment, string, Comment>(
                    sqlQuery,
                    (comment, userName) =>
                    {
                        comment.UserName = userName;
                        return comment;
                    },
                    new { postId, branchId, lastCommentId },
                    splitOn: "Name");
        }

		public async Task<int> MarkCommentAsDeletedAsync(int commentId, int userId)
		{
            const string sqlQuery = @"UPDATE Comments
									  SET Text = '', IsDeleted = 1
									  WHERE Id = @commentId AND UserId = @userId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.ExecuteAsync(sqlQuery, new { commentId, userId });
        }
	}
}