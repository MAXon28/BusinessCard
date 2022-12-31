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
    public class ChannelRepository : StandardRepository<Channel>, IChannelRepository
    {
        public ChannelRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<Channel> GetChannelDataAsync(int id)
        {
			const string sqlQuery = @"SELECT  TOP 28
											  channel.*,
											  post.*,
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
										FROM Channels channel
											INNER JOIN Posts post
											ON channel.Id = post.ChannelId
										WHERE channel.Id = @id
										ORDER BY post.Id DESC";

			using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            var channelsDictionary = new Dictionary<int, Channel>();

            var channel = (await dbConnection.QueryAsync<Channel, Post, int, int, int, int, Channel>(
                    sqlQuery,
                    (channel, post, topchiksCount, bookmarksCount, viewsCount, commentsCount) =>
                    {
                        if (!channelsDictionary.TryGetValue(channel.Id, out var channelInDictionary))
                        {
                            channelInDictionary = channel;
                            channelInDictionary.Posts = new List<Post>();
                            channelsDictionary.Add(channelInDictionary.Id, channelInDictionary);
                        }

                        post.ChannelName = channel.Name;
                        post.TopchiksCount = topchiksCount;
                        post.BookmarksCount = bookmarksCount;
                        post.ViewsCount = viewsCount;
                        post.CommentsCount = commentsCount;

                        channelInDictionary.Posts.Add(post);

                        return channelInDictionary;
                    },
                    new { id },
                    splitOn: "Id,Id,TopchiksCount,BookmarksCount,ViewsCount,CommentsCount"))
            .Distinct()
            .FirstOrDefault();

            return channel;
        }
    }
}