using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using Dapper;
using DapperAssistant;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonBlog
{
    /// <inheritdoc cref="IBookmarkRepository"/>
    public class BookmarkRepository : StandardRepository<Bookmark>, IBookmarkRepository
    {
        public BookmarkRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<int> DeletePostFromBookmarkAsync(int userId, int postId)
        {
            var sqlQuery = @"DELETE FROM Bookmarks
                             WHERE UserId = @userId AND PostId = @postId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.ExecuteAsync(sqlQuery, new { userId, postId });
        }
    }
}