using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonBlog
{
    internal class CommentBranchRepository : StandardRepository<CommentBranch>, ICommentBranchRepository
    {
        public CommentBranchRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<Dictionary<int, int>> GetCommentsCountInBranchesAsync(IEnumerable<int> branchesId)
        {
            const string sqlQuery = @"SELECT commentBranch.Id,
                                             COUNT(comment.Id) AS Count
                                      FROM CommentBranches commentBranch
	                                        INNER JOIN Comments comment
	                                        ON commentBranch.Id = comment.BranchId
                                      WHERE commentBranch.Id IN @branchesId
                                      GROUP BY commentBranch.Id";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return (await dbConnection.QueryAsync<int, int, KeyValuePair<int, int>> (
                    sqlQuery,
                    (branchId, commentsCount) => new KeyValuePair<int, int>(branchId, commentsCount),
                    new { branchesId },
                    splitOn: "Count")).ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}