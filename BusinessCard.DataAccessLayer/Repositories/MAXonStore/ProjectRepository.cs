using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using Dapper;
using DapperAssistant;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore
{
    /// <inheritdoc cref="IProjectRepository"/>
    public class ProjectRepository : StandardRepository<Project>, IProjectRepository
    {
        public ProjectRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) { }

        public async Task<IEnumerable<Project>> GetProjectsAsync(string sqlQuery, DynamicParameters parameters)
        {
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            var projects = await dbConnection.QueryAsync<Project, ProjectType, ProjectCategory, int, double, string, Project>(
                    sqlQuery,
                    (project, projectType, projectCategory, reviewsCount, rating, compatibilities) =>
                    {
                        project.ProjectType = projectType;
                        project.ProjectCategory = projectCategory;
                        project.ReviewsCount = reviewsCount;
                        project.Rating = rating;
                        project.Compatibilities = compatibilities.Split(',').ToList();
                        return project;
                    },
                    parameters,
                    splitOn: "Id,Id,Id,CountReviews,AvgRating,ProjectCompatibilities");

            return projects;
        }

        public async Task<int> GetProjectsCountAsync(string sqlQuery, DynamicParameters parameters)
        {
            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QuerySingleAsync<int>(sqlQuery, parameters);
        }


        public async Task<ProjectsInformation> GetProjectInformationAsync()
        {
            const string sqlQuery = @"SELECT  COUNT(project.Id) AS ProjectsCount,
		                                      SUM(project.ClicksCount) AS AllClicksCount,
		                                      (SELECT AVG (CONVERT (float, projectReview.Rating))
		                                       FROM ProjectReviews projectReview) AS AvgRatingAllProjects
                                      FROM Projects project;";


            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return (await dbConnection.QueryAsync<int, int, double, ProjectsInformation>(sqlQuery,
                (projectsCount, allDownloadsCount, allProjectsAvgRating) =>
                {
                    return new ProjectsInformation
                    {
                        ProjectsCount = projectsCount,
                        AllDownloadsCount = allDownloadsCount,
                        AllProjectsAvgRating = allProjectsAvgRating
                    };
                },
                splitOn: "ProjectsCount,AllClicksCount,AvgRatingAllProjects")).FirstOrDefault();
        }
    }
}