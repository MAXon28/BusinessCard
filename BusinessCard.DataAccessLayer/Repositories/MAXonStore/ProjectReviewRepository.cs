using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore
{
    /// <inheritdoc cref="IProjectReviewRepository"/>
    public class ProjectReviewRepository : StandardRepository<ProjectReview>, IProjectReviewRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly DbConnectionKeeper _dbConnectionKeeper;

        public ProjectReviewRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) => _dbConnectionKeeper = dbConnectionKeeper;

        public async Task<Dictionary<int, int>> GetReviewStatisticAsync(int projectId)
        {
            const string sqlQuery = @"SELECT Rating,
                                             COUNT(*) AS Count
                                      FROM ProjectReviews
                                      WHERE ProjectId = @projectId
                                      GROUP BY Rating";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return new Dictionary<int, int>(await dbConnection.QueryAsync<int, int, KeyValuePair<int, int>>(
                    sqlQuery,
                    (rating, ratingCount) => new KeyValuePair<int, int>(rating, ratingCount),
                    new { projectId },
                    splitOn: "Rating, Count"));
        }
    }
}