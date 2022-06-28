using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore
{
    /// <inheritdoc cref="IProjectCompatibilityRepository"/>
    public class ProjectCompatibilityRepository : StandardRepository<ProjectCompatibility>, IProjectCompatibilityRepository
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly DbConnectionKeeper _dbConnectionKeeper;

        public ProjectCompatibilityRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) => _dbConnectionKeeper = dbConnectionKeeper;

        public async Task<IEnumerable<string>> GetCompatibilitiesByProjectIdAsync(int projectId)
        {
            const string sqlQuery = @"SELECT comp.Name
                                      FROM ProjectCompatibilities comp
	                                    INNER JOIN ProjectsCompatibilities projComp
	                                    ON projComp.CompatibilityId = comp.Id
                                      WHERE projComp.ProjectId = @projectId";

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QueryAsync<string>(sqlQuery, new { projectId });
        }
    }
}