using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using Dapper;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonBlog
{
    /// <inheritdoc cref="ITopchikRepository"/>
    public class TopchikRepository : StandardRepository<Topchik>, ITopchikRepository
    {
        public TopchikRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<int> DeleteTopchikAsync(int userId, int postId)
        {
            var sqlQuery = @"DELETE FROM Topchiks
                             WHERE UserId = @userId AND PostId = @postId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.ExecuteAsync(sqlQuery, new { userId, postId });
        }
    }
}