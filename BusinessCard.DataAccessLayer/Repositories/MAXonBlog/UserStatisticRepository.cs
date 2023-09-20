using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonBlog
{
    /// <inheritdoc cref="IUserStatisticRepository"/>
    internal class UserStatisticRepository : IUserStatisticRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private DbConnectionKeeper _dbConnectionKeeper;

        public UserStatisticRepository(DbConnectionKeeper dbConnectionKeeper) => _dbConnectionKeeper = dbConnectionKeeper;

        public async Task<Dictionary<string, bool>> GetUserSubscriptionsByPostsAsync(int userId, int channelId)
        {
            const string onChannel = "OnChannel";
            const string onMailing = "OnMailing";

            var sqlQuery = @$"SELECT channelSubscribe.Id,
                                     '{onChannel}' AS TypeOfSubscribe
                              FROM ChannelSubscriptions channelSubscribe
                              WHERE channelSubscribe.UserId = @userId AND channelSubscribe.ChannelId = @channelId
                              UNION
                              SELECT mailingSubscribe.Id,
                                     '{onMailing}' AS TypeOfSubscribe
                              FROM MailingSubscriptions mailingSubscribe
                              WHERE mailingSubscribe.UserId = @userId AND mailingSubscribe.ChannelId = @channelId";

            var userSubscriptions = new Dictionary<string, bool>
            {
                [onChannel] = false,
                [onMailing] = false
            };

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            await dbConnection.QueryAsync<int, string, int>(
                    sqlQuery,
                    (id, typeOfSubscribe) =>
                    {
                        userSubscriptions[typeOfSubscribe] = true;
                        return id;
                    },
                    new { userId, channelId },
                    splitOn: "Id, TypeOfSubscribe");

            return userSubscriptions;
        }

        public async Task<Dictionary<int, Dictionary<string, bool>>> GetUserStatisticByPostsAsync(int userId, IEnumerable<int> postsId)
        {
            const string sqlQuery = @"SELECT  t.PostId,
		                                      'Topchiks' AS TypeOfStatistic
                                      FROM Topchiks t
                                      WHERE t.UserId = @userId AND t.PostId IN @postsId
                                      UNION
                                      SELECT  b.PostId,
		                                      'Bookmarks' AS TypeOfStatistic
                                      FROM Bookmarks b
                                      WHERE b.UserId = @userId AND b.PostId IN @postsId";

            const string topchiks = "Topchiks";
            const string bookmarks = "Bookmarks";
            var userStatistic = new Dictionary<int, Dictionary<string, bool>>();
            foreach (var postId in postsId)
                userStatistic.Add(postId, new Dictionary<string, bool>
                {
                    [topchiks] = false,
                    [bookmarks] = false
                });

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            await dbConnection.QueryAsync<int, string, int>(
                    sqlQuery,
                    (postId, typeOfStatistic) =>
                    {
                        userStatistic[postId][typeOfStatistic] = true;
                        return postId;
                    },
                    new { userId, postsId },
                    splitOn: "PostId, TypeOfStatistic");

            return userStatistic;
        }

        public async Task<Dictionary<string, int>> GetUserStatisticInBlogAsync(int userId)
        {
            const string sqlQuery = @"SELECT COUNT(Id),
	                                        'ChannelSubscriptionsCount' AS Name
                                        FROM ChannelSubscriptions
                                        WHERE UserId = @userId
                                        UNION
                                        SELECT COUNT(Id),
	                                        'MailingSubscriptionsCount' AS Name
                                        FROM MailingSubscriptions
                                        WHERE UserId = @userId
                                        UNION
                                        SELECT COUNT(Id),
	                                        'BookmarksCount' AS Name
                                        FROM Bookmarks
                                        WHERE UserId = @userId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return (await dbConnection.QueryAsync<int, string, KeyValuePair<string, int>>(
                    sqlQuery,
                    (count, name) => new KeyValuePair<string, int>(name, count),
                    new { userId },
                    splitOn: "Name")).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}