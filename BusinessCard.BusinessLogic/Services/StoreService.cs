using BusinessCard.BusinessLogicLayer.DTOs.Store;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using BusinessCard.DataAccessLayer.Repositories.MAXonStore.QueryHelper;
using DapperAssistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IStoreService"/>
    public class StoreService : IStoreService
    {
        /// <summary>
        /// Репозиторий проектов
        /// </summary>
        private readonly IProjectRepository _projectRepository;

        /// <summary>
        /// Репозиторий типов проектов
        /// </summary>
        private readonly IProjectTypeRepository _projectTypeRepository;

        /// <summary>
        /// Репозиторий категорий проектов
        /// </summary>
        private readonly IProjectCategoryRepository _projectCategoryRepository;

        /// <summary>
        /// Репозиторий совместимостей проектов
        /// </summary>
        private readonly IProjectCompatibilityRepository _projectCompatibilityRepository;

        /// <summary>
        /// Репозиторий изображений проектов
        /// </summary>
        private readonly IProjectImageRepository _projectImageRepository;

        /// <summary>
        /// Сервис отзывов по проектам
        /// </summary>
        private readonly IProjectReviewService _projectReviewService;

        /// <summary>
        /// Репозиторий технических требований к проектам
        /// </summary>
        private readonly IProjectTechnicalRequirementValueRepository _projectTechnicalRequirementValueRepository;

        public StoreService
            (IProjectRepository projectRepository,
            IProjectTypeRepository projectTypeRepository, 
            IProjectCategoryRepository projectCategoryRepository, 
            IProjectCompatibilityRepository projectCompatibilityRepository,
            IProjectImageRepository projectImageRepository,
            IProjectReviewService projectReviewService,
            IProjectTechnicalRequirementValueRepository projectTechnicalRequirementValueRepository)
        {
            _projectRepository = projectRepository;
            _projectTypeRepository = projectTypeRepository;
            _projectCategoryRepository = projectCategoryRepository;
            _projectCompatibilityRepository = projectCompatibilityRepository;
            _projectImageRepository = projectImageRepository;
            _projectReviewService = projectReviewService;
            _projectTechnicalRequirementValueRepository = projectTechnicalRequirementValueRepository;
        }

        public async Task<List<ProjectDto>> GetProjectsAsync(FiltersDtoIn filters, int projectsPackageNumber)
        {
            var projectQuerySettings = GetProjectQuerySettings(filters, projectsPackageNumber);

            var projects = await _projectRepository.GetProjectsAsync(projectQuerySettings);

            return GetProjectsDto(projects);
        }

        public async Task<ProjectsInformationDto> GetProjectsInformationAsync(FiltersDtoIn filters, int projectsPackageNumber)
        {
            var projectQuerySettings = GetProjectQuerySettings(filters, projectsPackageNumber);

            var projects = await _projectRepository.GetProjectsAsync(projectQuerySettings);

            var projectsCountByCurrentFilter = await _projectRepository.GetProjectsCountAsync(projectQuerySettings);

            return new ProjectsInformationDto
                {
                    PagesCountByCurrentFilters = GetPagesCount(projectsCountByCurrentFilter),
                    Projects = GetProjectsDto(projects)
                };
        }

        /// <summary>
        /// Получить настройки запроса по проектам
        /// </summary>
        /// <param name="filters"> Фильтры запроса </param>
        /// <param name="projectsPackageNumber"> Номер пакета проектов (в каждом пакете по 5 проектов) </param>
        /// <returns> Настройки запроса по проектам </returns>
        private ProjectQuerySettings GetProjectQuerySettings(FiltersDtoIn filters, int projectsPackageNumber)
        {
            var projectName = string.IsNullOrEmpty(filters.ProjectName) ? null : filters.ProjectName;
            var sortType = filters.SortType;
            var projectTypeFilters = filters.ProjectTypes is null || filters.ProjectTypes.Count == 0 ? null : filters.ProjectTypes;
            var projectCategoryFilters = filters.ProjectCategories is null || filters.ProjectCategories.Count == 0 ? null : filters.ProjectCategories;
            var projectCompatibilityFilters = filters.ProjectCompatibilities is null || filters.ProjectCompatibilities.Count == 0 ? null : filters.ProjectCompatibilities;
            var offset = GetOffset(projectsPackageNumber);
            return new ProjectQuerySettings(projectName, projectTypeFilters, projectCategoryFilters, projectCompatibilityFilters, sortType, offset);
        }

        /// <summary>
        /// Получить сдвиг проектов на конкретное число (для пагинации)
        /// </summary>
        /// <param name="projectsPackageNumber"> Номер пакета проектов (в каждом пакете по 5 проектов) </param>
        /// <returns> Сдвиг проектов на конкретное число (для пагинации) </returns>
        private int GetOffset(int projectsPackageNumber)
        {
            const int projectsCountPackage = 5;

            return (projectsPackageNumber - 1) * projectsCountPackage;
        }

        /// <summary>
        /// Получить список проектов в подготовленном виде для передачи на уровень представления
        /// </summary>
        /// <param name="projects"> Список проектов из базы данных </param>
        /// <returns> Список проектов в подготовленном виде для передачи на уровень представления </returns>
        private List<ProjectDto> GetProjectsDto(IEnumerable<Project> projects)
            => projects.Select(project =>
                        new ProjectDto
                        {
                            Id = project.Id,
                            Name = project.Name,
                            Type = project.ProjectType.Name,
                            Category = project.ProjectCategory.Name,
                            Compatibilities = GetRequiredCompatibilitiesVariant(project.Compatibilities),
                            ReviewsCount = project.ReviewsCount,
                            AvgRating = Math.Round(project.Rating, 1),
                            Icon = project.Icon
                        }).ToList();

        /// <summary>
        /// Получить необходимый формат совместимостей проекта
        /// </summary>
        /// <param name="projectCompatibilities"> Совместимости проекта </param>
        /// <returns> Необходимый формат совместимостей проекта </returns>
        private List<string> GetRequiredCompatibilitiesVariant(IEnumerable<string> projectCompatibilities)
            => projectCompatibilities.Select(projectCompatibility => ProcessValue(projectCompatibility)).ToList();

        public async Task<FiltersDtoOut> GetFiltersAsync()
        {
            var projectTypes = await _projectTypeRepository.GetAsync();
            var projectCategories = await _projectCategoryRepository.GetAsync();
            var projectCompatibilities = await _projectCompatibilityRepository.GetAsync();

            var filters = new FiltersDtoOut
            {
                ProjectTypes = projectTypes.Select(projectType =>
                    new FilterDtoOut
                    {
                        Id = projectType.Id,
                        Value = projectType.Name,
                        ProcessedValue = ProcessValue(projectType.Name)
                    }).ToList(),

                ProjectCategories = projectCategories.Select(projectCategory =>
                    new FilterDtoOut
                    {
                        Id = projectCategory.Id,
                        Value = projectCategory.Name,
                        ProcessedValue = ProcessValue(projectCategory.Name)
                    }).ToList(),

                ProjectCompatibilities = new Dictionary<string, List<FilterDtoOut>>()
            };

            foreach (var projectCompatibility in projectCompatibilities)
            {
                if (!filters.ProjectCompatibilities.TryGetValue(projectCompatibility.CompatibilitySection.SectionName, out _))
                    filters.ProjectCompatibilities.Add(projectCompatibility.CompatibilitySection.SectionName, new List<FilterDtoOut>());

                filters.ProjectCompatibilities[projectCompatibility.CompatibilitySection.SectionName].Add(
                    new FilterDtoOut
                    {
                        Id = projectCompatibility.Id,
                        Value = projectCompatibility.Name,
                        ProcessedValue = ProcessValue(projectCompatibility.Name)
                    });
            }

            return filters;
        }

        /// <summary>
        /// Обработать значение
        /// </summary>
        /// <param name="filterValue"> Значение </param>
        /// <returns> Обработанное значение (убраны все пробелы и все символы переведены в нижний регистр) </returns>
        private string ProcessValue(string filterValue) => filterValue.Replace(" ", string.Empty).ToLower();

        public async Task<GeneralInformationDto> GetGeneralInformationAsync()
        {
            var projectInformation = await _projectRepository.GetProjectInformationAsync();

            return new GeneralInformationDto
            {
                ProjectsCount = new CountInformationDto
                                {
                                    Count = projectInformation.ProjectsCount,
                                    Text = new ProjectWordEndingService().GetWord(projectInformation.ProjectsCount)
                                },
                DownloadsCount = new CountInformationDto
                                {
                                    Count = projectInformation.AllDownloadsCount,
                                    Text = new DownloadWordEndingService().GetWord(projectInformation.AllDownloadsCount)
                                },
                AvgRating = Math.Round(projectInformation.AllProjectsAvgRating, 1),
                PagesCount = GetPagesCount(projectInformation.ProjectsCount)
            };
        }

        /// <summary>
        /// Получить количество страниц с проектами
        /// </summary>
        /// <param name="projectsCount"> Количество проектов </param>
        /// <returns> Количество страниц с проектами </returns>
        private int GetPagesCount(int projectsCount)
        {
            const int projectsCountPackage = 5;

            return projectsCount / projectsCountPackage + (projectsCount % projectsCountPackage > 0 ? 1 : 0);
        }

        public async Task<ProjectDto> GetProjectInformationAsync(int projectId)
        {
            var projectTask = GetProjectAsync(projectId);
            var projectImagesTask = GetProjectImagesAsync(projectId);
            var ratingStatisticTask = _projectReviewService.GetRatingStatisticAsync(projectId);
            var technicalRequirementsTask = GetTechnicalRequirementsAsync(projectId);
            var compatibilitiesTask = GetCompatibilitiesAsync(projectId);

            var projectInformation = new ProjectDto();
            SetProjectData(projectInformation, await projectTask);
            SetProjectImages(projectInformation, await projectImagesTask);
            SetProjectStatistic(projectInformation, await ratingStatisticTask, projectId);
            SetTechnicalRequirements(projectInformation, await technicalRequirementsTask, await compatibilitiesTask);

            return projectInformation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <returns>  </returns>
        private async Task<Project> GetProjectAsync(int projectId)
        {
            var querySettings = new QuerySettings
            {
                ConditionField = "Id",
                ConditionType = ConditionType.EQUALLY,
                ConditionFieldValue = projectId
            };

            return (await _projectRepository.GetWithConditionAsync(querySettings)).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <returns>  </returns>
        private async Task<IEnumerable<ProjectImage>> GetProjectImagesAsync(int projectId)
        {
            var querySettings = new QuerySettings
            {
                ConditionField = "ProjectId",
                ConditionType = ConditionType.EQUALLY,
                ConditionFieldValue = projectId
            };

            return await _projectImageRepository.GetWithConditionAsync(querySettings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <returns>  </returns>
        private async Task<IEnumerable<string>> GetCompatibilitiesAsync(int projectId) => await _projectCompatibilityRepository.GetCompatibilitiesByProjectIdAsync(projectId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId">  </param>
        /// <returns>  </returns>
        private async Task<IEnumerable<ProjectTechnicalRequirementValue>> GetTechnicalRequirementsAsync(int projectId)
        {
            var querySettings = new QuerySettings
            {
                ConditionField = "ProjectId",
                ConditionType = ConditionType.EQUALLY,
                ConditionFieldValue = projectId
            };

            return await _projectTechnicalRequirementValueRepository.GetWithConditionAsync(querySettings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectInformation">  </param>
        /// <param name="project">  </param>
        private void SetProjectData(ProjectDto projectInformation, Project project)
        {
            projectInformation.Name = project.Name;
            projectInformation.Type = project.ProjectType.Name;
            projectInformation.Category = project.ProjectCategory.Name;
            projectInformation.ClicksCount = project.ClicksCount;
            projectInformation.ProjectUrl = project.ProjectUrl;
            projectInformation.Description = project.Description;
            projectInformation.VideoUrl = project.VideoUrl;
            projectInformation.CodeUrl = project.CodeUrl;
            projectInformation.CreationDate = project.CreationDate.ToString("Y");
            projectInformation.Icon = project.Icon;
            projectInformation.ClickType = project.ClickType is null ? null : GetClickTypeDto(project.ClickType);
            projectInformation.CodeLevel = GetCodeLevelDto(project.CodeLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clickType">  </param>
        /// <returns>  </returns>
        private ClickTypeDto GetClickTypeDto(ClickType clickType)
            => new ClickTypeDto
            {
                TypeName = clickType.TypeName,
                ActionName = clickType.ActionName
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeLevel">  </param>
        /// <returns>  </returns>
        private CodeLevelDto GetCodeLevelDto(CodeLevel codeLevel)
            => new CodeLevelDto
            {
                Percentage = codeLevel.Percentage,
                Annotation = codeLevel.Annotation
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectInformation">  </param>
        /// <param name="projectImages">  </param>
        private void SetProjectImages(ProjectDto projectInformation, IEnumerable<ProjectImage> projectImages) => projectInformation.Images = GetProjectImagesDto(projectImages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectImages">  </param>
        /// <returns>  </returns>
        private List<ProjectImageDto> GetProjectImagesDto(IEnumerable<ProjectImage> projectImages)
            => projectImages.Select(image => new ProjectImageDto
            {
                Path = image.Path,
                Description = image.Description
            }).ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectInformation">  </param>
        /// <param name="ratingStatistic">  </param>
        /// <param name="projectId">  </param>
        private void SetProjectStatistic(ProjectDto projectInformation, Dictionary<int, int> ratingStatistic, int projectId)
        {
            projectInformation.ReviewsCount = ratingStatistic.Sum(statistic => statistic.Value);
            projectInformation.AvgRating = Math.Round(ratingStatistic.Sum(rating => rating.Key * rating.Value) / (double) projectInformation.ReviewsCount, 1);
            projectInformation.RatingStatistic = new RatingStatisticDto
            {
                ExcellentCount = ratingStatistic.ContainsKey(5) ? (int)(Math.Round(ratingStatistic[5] / (double)projectInformation.ReviewsCount, 2) * 100) : 0,
                GoodCount = ratingStatistic.ContainsKey(4) ? (int)(Math.Round(ratingStatistic[4] / (double)projectInformation.ReviewsCount, 2) * 100) : 0,
                NotBadCount = ratingStatistic.ContainsKey(3) ? (int)(Math.Round(ratingStatistic[3] / (double)projectInformation.ReviewsCount, 2) * 100) : 0,
                BadCount = ratingStatistic.ContainsKey(2) ? (int)(Math.Round(ratingStatistic[2] / (double)projectInformation.ReviewsCount, 2) * 100) : 0,
                TerriblyCount = ratingStatistic.ContainsKey(0) ? (int)(Math.Round(ratingStatistic[1] / (double)projectInformation.ReviewsCount, 2) * 100) : 0
            };

            if (projectInformation.ReviewsCount > 0)
                projectInformation.Review = _projectReviewService.GetReviewAsync(projectId).Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectInformation">  </param>
        /// <param name="projectTechnicalRequirementValues">  </param>
        /// <param name="compatibilities">  </param>
        private void SetTechnicalRequirements(ProjectDto projectInformation, IEnumerable<ProjectTechnicalRequirementValue> projectTechnicalRequirementValues, IEnumerable<string> compatibilities)
        {
            var technicalRequirements = new Dictionary<string, string>();

            foreach (var requirement in projectTechnicalRequirementValues)
            {
                var name = requirement.ProjectTechnicalRequirementName.Name;

                if (technicalRequirements.ContainsKey(name))
                {
                    technicalRequirements[name] = string.Join(", ", requirement.Value);
                    continue;
                }
                technicalRequirements.Add(name, string.Join(", ", requirement.Value));
            }
            technicalRequirements.Add("Совместимость", string.Join(", ", compatibilities));

            projectInformation.TechnicalRequirements = technicalRequirements;
        }
    }
}