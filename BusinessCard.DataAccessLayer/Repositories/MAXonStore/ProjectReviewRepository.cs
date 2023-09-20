using BusinessCard.DataAccessLayer.Entities.Data;
using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore
{
    /// <inheritdoc cref="IProjectReviewRepository"/>
    internal class ProjectReviewRepository : StandardRepository<ProjectReview>, IProjectReviewRepository
    {
        public ProjectReviewRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<IEnumerable<ProjectReview>> GetReviewsAsync(int projectId, int offset)
        {
            const string sqlQuery = @"SELECT review.*,
                                             userData.Name
                                      FROM ProjectReviews review
                                        LEFT JOIN Users userData
                                        ON review.UserId = userData.Id
                                      WHERE review.ProjectId = @projectId
                                      ORDER BY review.ID DESC
                                        OFFSET @offset ROWS
										FETCH NEXT 28 ROWS ONLY";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QueryAsync<ProjectReview, User, ProjectReview>(
                    sqlQuery,
                    (projectReview, user) =>
                    {
                        projectReview.User = user;
                        return projectReview;
                    },
                    new { projectId, offset },
                    splitOn: "Id, Name");
        }

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