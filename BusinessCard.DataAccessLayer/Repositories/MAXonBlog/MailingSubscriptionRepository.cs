using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using Dapper;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonBlog
{
    /// <inheritdoc cref="IMailingSubscriptionRepository"/>
    public class MailingSubscriptionRepository : StandardRepository<MailingSubscription>, IMailingSubscriptionRepository
    {
        public MailingSubscriptionRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<int> DeleteChannelFromSubscriptionAsync(int userId, int channelId)
        {
            var sqlQuery = @"DELETE FROM MailingSubscriptions
                             WHERE UserId = @userId AND ChannelId = @channelId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.ExecuteAsync(sqlQuery, new { userId, channelId });
        }
    }
}