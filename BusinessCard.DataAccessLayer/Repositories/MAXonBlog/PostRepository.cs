using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonBlog
{
    /// <summary>
    /// 
    /// </summary>
    public class PostRepository : StandardRepository<Post>, IPostRepository
    {
        public PostRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<IEnumerable<Post>> GetPostsAsync(int offset)
        {
            const string sqlQuery = @"SELECT  post.*,
											  channel.Name,
											  (SELECT COUNT(*)
											   FROM Topchiks topchik
											   WHERE topchik.PostId = post.Id) AS TopchiksCount,
											   (SELECT COUNT(*)
											   FROM Bookmarks bookmark
											   WHERE bookmark.PostId = post.Id) AS BookmarksCount,
											   (SELECT COUNT(*)
											   FROM PostViews postView
											   WHERE postView.PostId = post.Id) AS ViewsCount
										FROM Posts post
											INNER JOIN Channels channel
											ON channel.Id = post.ChannelId
										ORDER BY post.Id DESC
                                        OFFSET @offset ROWS
										FETCH NEXT 28 ROWS ONLY";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QueryAsync<Post, string, int, int, int, Post>(
                    sqlQuery,
                    (post, channelName, topchiksCount, bookmarksCount, viewsCount) =>
                    {
                        post.ChannelName = channelName;
                        post.TopchiksCount = topchiksCount;
                        post.BookmarksCount = bookmarksCount;
                        post.ViewsCount = viewsCount;
                        return post;
                    },
                    new { offset },
                    splitOn: "Id, Name");
        }
    }
}