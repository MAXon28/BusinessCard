using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using BusinessCard.DataAccessLayer.Repositories.MAXonStore.QueryHelper;
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
        /// <summary>
        /// 
        /// </summary>
        private readonly DbConnectionKeeper _dbConnectionKeeper;

        public ProjectRepository(DbConnectionKeeper dbConnectionKeeper) : base(dbConnectionKeeper) => _dbConnectionKeeper = dbConnectionKeeper;

        public async Task<IEnumerable<Project>> GetProjectsAsync(ProjectQuerySettings projectQuerySettings)
        {
            var projectQueryData = new ProjectsSelectionQueryBuilder().GetProjectQueryData(projectQuerySettings, TypeOfSelect.Projects);

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();
            var projects = await dbConnection.QueryAsync<Project, ProjectType, ProjectCategory, int, double, string, Project>(
                    projectQueryData.SqlQuery,
                    (project, projectType, projectCategory, reviewsCount, rating, compatibilities) =>
                    {
                        project.ProjectType = projectType;
                        project.ProjectCategory = projectCategory;
                        project.ReviewsCount = reviewsCount;
                        project.Rating = rating;
                        project.Compatibilities = compatibilities.Split(',').ToList();
                        return project;
                    },
                    projectQueryData.Parameters,
                    splitOn: "Id,Id,Id,CountReviews,AvgRating,ProjectCompatibilities");

            return projects;
        }

        public async Task<int> GetProjectsCountAsync(ProjectQuerySettings projectQuerySettings)
        {
            var projectQueryData = new ProjectsSelectionQueryBuilder().GetProjectQueryData(projectQuerySettings, TypeOfSelect.Count);

            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return await dbConnection.QuerySingleAsync<int>(projectQueryData.SqlQuery, projectQueryData.Parameters);
        }


        public async Task<ProjectInformation> GetProjectInformationAsync()
        {
            const string sqlQuery = @"SELECT  COUNT(project.Id) AS ProjectsCount,
		                                      SUM(project.ClicksCount) AS AllClicksCount,
		                                      (SELECT AVG (CONVERT (float, projectReview.Rating))
		                                       FROM ProjectReviews projectReview) AS AvgRatingAllProjects
                                      FROM Projects project;";


            using var dbConnection = _dbConnectionKeeper.GetDbConnection();

            return (await dbConnection.QueryAsync<int, int, double, ProjectInformation>(sqlQuery,
                (projectsCount, allDownloadsCount, allProjectsAvgRating) =>
                {
                    return new ProjectInformation
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