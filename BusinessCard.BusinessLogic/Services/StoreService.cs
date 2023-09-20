using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Interfaces.Utils;
using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Utils.Attributes;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore;
using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using BusinessCard.Entities.DTO.Store;
using DapperAssistant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IStoreService"/>
    internal class StoreService : IStoreService
    {
        /// <summary>
        /// Количество проектов в одном пакете (для пагинации)
        /// </summary>
        private const int ProjectsCountInPackage = 5;

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
        /// Репозиторий технических требований к проектам
        /// </summary>
        private readonly IProjectTechnicalRequirementValueRepository _projectTechnicalRequirementValueRepository;

        /// <summary>
        /// Сервис отзывов по проектам
        /// </summary>
        private readonly IProjectReviewService _projectReviewService;

        /// <summary>
        /// Строитель запросов выборки
        /// </summary>
        private readonly ISelectionQueryBuilder _selectionQueryBuilder;

        /// <summary>
        /// Утилита для пагинации по проектам
        /// </summary>
        private readonly IPagination _pagination;

        /// <summary>
        /// Сервис для окончания слова (правильность написания)
        /// </summary>
        private readonly IWordEnding _wordEnding;

        public StoreService
            (IProjectRepository projectRepository,
            IProjectTypeRepository projectTypeRepository,
            IProjectCategoryRepository projectCategoryRepository,
            IProjectCompatibilityRepository projectCompatibilityRepository,
            IProjectImageRepository projectImageRepository,
            IProjectReviewService projectReviewService,
            IProjectTechnicalRequirementValueRepository projectTechnicalRequirementValueRepository,
            ISelectionQueryBuilderFactory selectionQueryBuilderFactory,
            IPagination pagination,
            IWordEnding wordEnding)
        {
            _projectRepository = projectRepository;
            _projectTypeRepository = projectTypeRepository;
            _projectCategoryRepository = projectCategoryRepository;
            _projectCompatibilityRepository = projectCompatibilityRepository;
            _projectImageRepository = projectImageRepository;
            _projectReviewService = projectReviewService;
            _projectTechnicalRequirementValueRepository = projectTechnicalRequirementValueRepository;
            _selectionQueryBuilder = selectionQueryBuilderFactory.GetQueryBuilder(QueryBuilderTypes.Projects);
            _pagination = pagination;
            _wordEnding = wordEnding;
        }

        public async Task<List<ProjectInformation>> GetProjectsAsync(FiltersIn filters, int projectsPackageNumber)
        {
            var projectRequestSettings = GetProjectRequestSettings(filters, projectsPackageNumber);
            var queryData = _selectionQueryBuilder.GetQueryData(projectRequestSettings);
            var projects = await _projectRepository.GetProjectsAsync(queryData.SqlQuery, queryData.Parameters);
            return GetProjectsDto(projects);
        }

        public async Task<ProjectsInformationDto> GetProjectsInformationAsync(FiltersIn filters, int projectsPackageNumber)
        {
            var projectRequestSettings = GetProjectRequestSettings(filters, projectsPackageNumber);

            var queryDataForProjects = _selectionQueryBuilder.GetQueryData(projectRequestSettings);
            var projects = await _projectRepository.GetProjectsAsync(queryDataForProjects.SqlQuery, queryDataForProjects.Parameters);
            _selectionQueryBuilder.TypeOfSelect = SelectTypes.Count;
            var queryDataForCount = _selectionQueryBuilder.GetQueryData(projectRequestSettings);
            var projectsCountByCurrentFilter = await _projectRepository.GetProjectsCountAsync(queryDataForCount.SqlQuery, queryDataForCount.Parameters);

            return new()
            {
                PagesCountByCurrentFilters = _pagination.GetPagesCount(ProjectsCountInPackage, projectsCountByCurrentFilter),
                Projects = GetProjectsDto(projects)
            };
        }

        /// <summary>
        /// Получить настройки запроса по проектам
        /// </summary>
        /// <param name="filters"> Фильтры запроса </param>
        /// <param name="projectsPackageNumber"> Номер пакета проектов (в каждом пакете по 5 проектов) </param>
        /// <returns> Настройки запроса по проектам </returns>
        private ProjectRequestSettings GetProjectRequestSettings(FiltersIn filters, int projectsPackageNumber)
        {
            var projectName = string.IsNullOrEmpty(filters.ProjectName) is false
                ? filters.ProjectName
                : null;

            var lastProjectId = filters.LastProjectId;

            var sortType = filters.SortType;

            var projectTypeFilters = filters.ProjectTypes is not null && filters.ProjectTypes.Count > 0
                ? filters.ProjectTypes
                : null;

            var projectCategoryFilters = filters.ProjectCategories is not null && filters.ProjectCategories.Count > 0
                ? filters.ProjectCategories
                : null;

            var projectCompatibilityFilters = filters.ProjectCompatibilities is not null && filters.ProjectCompatibilities.Count > 0
                ? filters.ProjectCompatibilities
                : null;

            var offset = filters.LastProjectId == -1
                ? _pagination.GetOffset(ProjectsCountInPackage, projectsPackageNumber)
                : -1;

            return new(lastProjectId, ProjectsCountInPackage, projectName, projectTypeFilters, projectCategoryFilters, projectCompatibilityFilters, sortType, offset);
        }

        /// <summary>
        /// Получить список проектов в подготовленном виде для передачи на уровень представления
        /// </summary>
        /// <param name="projects"> Список проектов из базы данных </param>
        /// <returns> Список проектов в подготовленном виде для передачи на уровень представления </returns>
        private List<ProjectInformation> GetProjectsDto(IEnumerable<Project> projects)
            => projects
                .Select(project =>
                    new ProjectInformation
                    {
                        Id = project.Id,
                        Name = project.Name,
                        Type = project.ProjectType.Name,
                        Category = project.ProjectCategory.Name,
                        Compatibilities = GetRequiredCompatibilitiesVariant(project.Compatibilities),
                        ReviewInformation = new ProjectReviewInformation
                        {
                            ReviewsCount = project.ReviewsCount,
                            AvgRating = Math.Round(project.Rating, 1)
                        },
                        Icon = project.Icon
                    })
                .ToList();

        /// <summary>
        /// Получить необходимый формат совместимостей проекта
        /// </summary>
        /// <param name="projectCompatibilities"> Совместимости проекта </param>
        /// <returns> Необходимый формат совместимостей проекта </returns>
        private List<string> GetRequiredCompatibilitiesVariant(IEnumerable<string> projectCompatibilities)
            => projectCompatibilities
                .Select(projectCompatibility => ProcessValue(projectCompatibility))
                .ToList();

        public async Task<FiltersOut> GetFiltersAsync()
        {
            var projectTypes = await _projectTypeRepository.GetAsync();
            var projectCategories = await _projectCategoryRepository.GetAsync();
            var projectCompatibilities = await _projectCompatibilityRepository.GetAsync();

            var filters = new FiltersOut
            {
                ProjectTypes = projectTypes
                    .Select(projectType =>
                        new FilterOut
                        {
                            Id = projectType.Id,
                            Value = projectType.Name,
                            ProcessedValue = ProcessValue(projectType.Name)
                        })
                    .ToList(),

                ProjectCategories = projectCategories
                    .Select(projectCategory =>
                        new FilterOut
                        {
                            Id = projectCategory.Id,
                            Value = projectCategory.Name,
                            ProcessedValue = ProcessValue(projectCategory.Name)
                        })
                    .ToList(),

                ProjectCompatibilities = new Dictionary<string, List<FilterOut>>()
            };

            foreach (var projectCompatibility in projectCompatibilities)
            {
                if (!filters.ProjectCompatibilities.TryGetValue(projectCompatibility.CompatibilitySection.SectionName, out _))
                    filters.ProjectCompatibilities.Add(projectCompatibility.CompatibilitySection.SectionName, new List<FilterOut>());

                filters.ProjectCompatibilities[projectCompatibility.CompatibilitySection.SectionName].Add(
                    new FilterOut
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
        private static string ProcessValue(string filterValue)
            => filterValue.Replace(" ", string.Empty).ToLower();

        public async Task<GeneralInformation> GetGeneralInformationAsync()
        {
            var projectInformation = await _projectRepository.GetProjectInformationAsync();

            return new()
            {
                ProjectsCount = GetCountInformation(projectInformation.ProjectsCount, WordsVariants.Project),
                DownloadsCount = GetCountInformation(projectInformation.AllDownloadsCount, WordsVariants.Download),
                AvgRating = Math.Round(projectInformation.AllProjectsAvgRating, 1),
                PagesCount = _pagination.GetPagesCount(ProjectsCountInPackage, projectInformation.ProjectsCount)
            };
        }

        /// <summary>
        /// Получить информацию по количеству данных
        /// </summary>
        /// <param name="number"> Количество данных </param>
        /// <param name="word"> Слово, по которому надо получить нужны вариант его написания в завивимости от количества </param>
        /// <returns> Информация по количеству данных </returns>
        private CountInformation GetCountInformation(int number, WordsVariants word)
        {
            _wordEnding.Init(word);
            return new()
            {
                Count = number,
                Text = _wordEnding.GetWord(number)
            };
        }

        public async Task<ProjectInformation> GetProjectInformationAsync(int projectId)
        {
            var projectTask = GetProjectAsync(projectId);
            var projectImagesTask = GetProjectImagesAsync(projectId);
            var reviewInformationTask = _projectReviewService.GetReviewInformationAsync(projectId, true);
            var technicalRequirementsTask = GetTechnicalRequirementsAsync(projectId);
            var compatibilitiesTask = GetCompatibilitiesAsync(projectId);

            var projectInformation = new ProjectInformation();
            SetProjectData(projectInformation, await projectTask);
            SetProjectImages(projectInformation, await projectImagesTask);
            SetProjectStatistic(projectInformation, await reviewInformationTask);
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
        private async Task<IEnumerable<string>> GetCompatibilitiesAsync(int projectId)
            => await _projectCompatibilityRepository.GetCompatibilitiesByProjectIdAsync(projectId);

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
        private void SetProjectData(ProjectInformation projectInformation, Project project)
        {
            projectInformation.Name = project.Name;
            projectInformation.Type = project.ProjectType.Name;
            projectInformation.Category = project.ProjectCategory.Name;
            projectInformation.ClicksCount = project.ClicksCount;
            projectInformation.ProjectUrl = project.ProjectUrl;
            projectInformation.Description = project.Description;
            projectInformation.VideoUrl = project.VideoUrl;
            projectInformation.CodeUrl = project.CodeUrl;
            projectInformation.CreationDate = project.CreationDate.ConvertToReadableFormat();
            projectInformation.Icon = project.Icon;
            projectInformation.ClickType = project.ClickType is not null ? GetClickTypeDto(project.ClickType) : null;
            projectInformation.CodeLevel = GetCodeLevelDto(project.CodeLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clickType">  </param>
        /// <returns>  </returns>
        private ClickTypeDto GetClickTypeDto(ClickType clickType)
            => new()
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
            => new()
            {
                Percentage = codeLevel.Percentage,
                Annotation = codeLevel.Annotation
            };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectInformation">  </param>
        /// <param name="projectImages">  </param>
        private void SetProjectImages(ProjectInformation projectInformation, IEnumerable<ProjectImage> projectImages)
            => projectInformation.Images = GetProjectImagesDto(projectImages);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectImages">  </param>
        /// <returns>  </returns>
        private List<ProjectImageDto> GetProjectImagesDto(IEnumerable<ProjectImage> projectImages)
            => projectImages
                .Select(image =>
                    new ProjectImageDto
                    {
                        Path = image.Path,
                        Description = image.Description
                    })
                .ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectInformation">  </param>
        /// <param name="reviewInformation">  </param>
        private static void SetProjectStatistic(ProjectInformation projectInformation, ProjectReviewInformation reviewInformation)
            => projectInformation.ReviewInformation = reviewInformation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectInformation">  </param>
        /// <param name="projectTechnicalRequirementValues">  </param>
        /// <param name="compatibilities">  </param>
        private void SetTechnicalRequirements(ProjectInformation projectInformation, IEnumerable<ProjectTechnicalRequirementValue> projectTechnicalRequirementValues, IEnumerable<string> compatibilities)
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