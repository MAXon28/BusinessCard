using BusinessCard.DataAccessLayer.Entities.MAXonBlog;
using BusinessCard.DataAccessLayer.Entities.MAXonBlog.PostDetails;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonBlog
{
    public class PostElementRepository : StandardRepository<PostElement>, IPostElementRepository
    {
        public PostElementRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<IEnumerable<PostElement>> GetPostElementsAsync(string postKey)
        {
            const string sqlQuery = @"SELECT postElement.Value,
                                             postElement.Description,
                                             postElement.Position,
                                             postField.Name
                                      FROM PostElements postElement
                                        INNER JOIN PostFields postField
                                        ON postField.Id = postElement.FieldId
                                      WHERE postElement.PostKey = @postKey";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QueryAsync<PostElement, string, PostElement>(
                    sqlQuery,
                    (postElement, fieldName) =>
                    {
                        postElement.FieldName = fieldName;
                        return postElement;
                    },
                    new { postKey },
                    splitOn: "Name");
        }
    }
}