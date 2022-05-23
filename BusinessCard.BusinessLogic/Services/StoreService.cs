using BusinessCard.BusinessLogicLayer.DTOs.Store;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Entities.MAXonStore;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using BusinessCard.DataAccessLayer.Repositories.MAXonStore.QueryHelper;
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

        public StoreService
            (IProjectRepository projectRepository,
            IProjectTypeRepository projectTypeRepository, 
            IProjectCategoryRepository projectCategoryRepository, 
            IProjectCompatibilityRepository projectCompatibilityRepository)
        {
            _projectRepository = projectRepository;
            _projectTypeRepository = projectTypeRepository;
            _projectCategoryRepository = projectCategoryRepository;
            _projectCompatibilityRepository = projectCompatibilityRepository;
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

            var filters = new FiltersDtoOut();

            filters.ProjectTypes = projectTypes.Select(projectType => 
                new FilterDtoOut
                {
                    Id = projectType.Id,
                    Value = projectType.Name,
                    ProcessedValue = ProcessValue(projectType.Name)
                }).ToList();

            filters.ProjectCategories = projectCategories.Select(projectCategory => 
                new FilterDtoOut
                {
                    Id = projectCategory.Id,
                    Value = projectCategory.Name,
                    ProcessedValue = ProcessValue(projectCategory.Name)
                }).ToList();

            filters.ProjectCompatibilities = new Dictionary<string, List<FilterDtoOut>>();
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
    }
}