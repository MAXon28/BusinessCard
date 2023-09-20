using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using Dapper;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonBlog
{
    /// <inheritdoc cref="IChannelSubscriptionRepository"/>
    internal class ChannelSubscriptionRepository : StandardRepository<ChannelSubscription>, IChannelSubscriptionRepository
    {
        public ChannelSubscriptionRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<int> DeleteChannelFromSubscriptionAsync(int userId, int channelId)
        {
            var sqlQuery = @"DELETE FROM ChannelSubscriptions
                             WHERE UserId = @userId AND ChannelId = @channelId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.ExecuteAsync(sqlQuery, new { userId, channelId });
        }
    }
}