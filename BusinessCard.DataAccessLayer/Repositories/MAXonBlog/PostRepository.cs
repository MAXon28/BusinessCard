using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    internal class PostRepository : StandardRepository<Post>, IPostRepository
    {
        public PostRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<IEnumerable<Post>> GetPostsAsync(string sqlQuery, DynamicParameters parameters)
        {
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QueryAsync<Post, string, int, int, int, int, Post>(
                    sqlQuery,
                    (post, channelName, topchiksCount, bookmarksCount, viewsCount, commentsCount) =>
                    {
                        post.ChannelName = channelName;
                        post.TopchiksCount = topchiksCount;
                        post.BookmarksCount = bookmarksCount;
                        post.ViewsCount = viewsCount;
                        post.CommentsCount = commentsCount;
                        return post;
                    },
                    parameters,
                    splitOn: "Id, Name, TopchiksCount, BookmarksCount, ViewsCount, CommentsCount");
        }

        public async Task<int> GetPostsCountAsync(string sqlQuery, DynamicParameters parameters)
        {
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            return await dbConnection.QuerySingleAsync<int>(sqlQuery, parameters);
        }

        public async Task<Post> GetPostAsync(string postKey)
        {
            const string sqlQuery = @"SELECT post.*,
										     channel.Name,
										     (SELECT COUNT(*)
										     FROM Topchiks topchik
										     WHERE topchik.PostId = post.Id) AS TopchiksCount,
										     (SELECT COUNT(*)
										     FROM Bookmarks bookmark
										     WHERE bookmark.PostId = post.Id) AS BookmarksCount,
										     (SELECT COUNT(*)
										     FROM PostViews postView
										     WHERE postView.PostId = post.Id) AS ViewsCount,
                                             (SELECT COUNT(*)
										     FROM Comments comment
                                                INNER JOIN CommentBranches branch
                                                ON branch.Id = comment.BranchId 
										     WHERE branch.PostId = post.Id) AS CommentsCount
                                      FROM Posts post
                                        INNER JOIN Channels channel
						                ON channel.Id = post.ChannelId
                                      WHERE post.PostKey = @postKey";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return (await dbConnection.QueryAsync<Post, string, int, int, int, int, Post>(
                    sqlQuery,
                    (post, channelName, topchiksCount, bookmarksCount, viewsCount, commentsCount) =>
                    {
                        post.ChannelName = channelName;
                        post.TopchiksCount = topchiksCount;
                        post.BookmarksCount = bookmarksCount;
                        post.ViewsCount = viewsCount;
                        post.CommentsCount = commentsCount;
                        return post;
                    },
                    new { postKey },
                    splitOn: "Id, Name, TopchiksCount, BookmarksCount, ViewsCount, CommentsCount")).FirstOrDefault();
        }
    }
}