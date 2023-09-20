using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore
{
    /// <summary>
    /// Строитель запроса выборки проектов
    /// </summary>
    internal class ProjectsSelectionQueryBuilder : SelectionQueryBuilder
    {
        /// <summary>
        /// Название таблицы
        /// </summary>
        private const string TableName = "Projects";

        /// <summary>
        /// Название переменной соответствующей таблицы
        /// </summary>
        private const string TableElementName = "project";

        /// <summary>
        /// Сдвиг на конкретное число (для пагинации)
        /// </summary>
        private int _offset;

        /// <summary>
        /// 
        /// </summary>
        private ProjectRequestSettings _projectRequestSettings;

        /// <summary>
        /// Нужна ли фильтрация по совместимости </param>
        /// </summary>
        private bool _needProjectCompatibilityFilter;

        public ProjectsSelectionQueryBuilder()
        {
            _filterBuildersDictionary = new Dictionary<string, IFilterBuilder>
            {
                [FilterConstants.Id] = new IdFilterBuilder(TableElementName),
                [FilterConstants.ProjectName] = new ProjectNameFilterBuilder(),
                [FilterConstants.ProjectType] = new ProjectTypeFilterBuilder(),
                [FilterConstants.ProjectCategory] = new ProjectCategoryFilterBuilder(),
                [FilterConstants.ProjectCompatibility] = new ProjectCompatibilityFilterBuilder()
            };
        }

        /// <inheritdoc/>
        public override QueryData GetQueryData(RequestSettings requestSettings)
        {
            _dataCountInPackage = requestSettings.CountInPackage;

            _projectRequestSettings = requestSettings as ProjectRequestSettings;
            _offset = _projectRequestSettings.Offset;
            _needProjectCompatibilityFilter = _projectRequestSettings.FilterValuesDictionary[FilterConstants.ProjectCompatibility] is not null;

            _sqlQuery = new StringBuilder();
            _sqlQuery.Append(_sqlQueryTemplate);
            SetTableNamings(TableName, TableElementName);
            SetSelect();
            SetJoin();
            SetWhere();
            if (TypeOfSelect == SelectTypes.Data)
            {
                SetTop(_offset);
                SetOrderBy();
                SetOrderBy(_projectRequestSettings.TypeOfSort);
            }

            return new(_sqlQuery.ToString(), _parameters);
        }

        /// <inheritdoc/>
        protected override void SetDataSelect()
        {
            var projectSelectionSet = $@"DISTINCT   {TableElementName}.Id,
										            {TableElementName}.Name,
										            {TableElementName}.Icon,
										            projectType.Id,
										            projectType.Name,
										            projectCategory.Id,
										            projectCategory.Name,
													(SELECT COUNT(*)
													FROM ProjectReviews projectReview
													WHERE projectReview.ProjectId = {TableElementName}.id
													GROUP BY projectReview.ProjectId) AS CountReviews,
										            (SELECT AVG(CONVERT (float, projectReview.Rating))
											        FROM ProjectReviews projectReview
											        WHERE projectReview.ProjectId = {TableElementName}.id
											        GROUP BY projectReview.ProjectId) AS AvgRating,
										            STUFF((SELECT CAST(',' AS VARCHAR(MAX)) + projectCompatibility.Name
												            FROM ProjectCompatibilities projectCompatibility
															INNER JOIN ProjectsCompatibilities projectCompatibilities
															ON projectCompatibilities.CompatibilityId = projectCompatibility.Id
												            WHERE projectCompatibilities.ProjectId = {TableElementName}.Id
												            FOR XML PATH('')), 1, 1, '') AS ProjectCompatibilities";

            const string distinct = "DISTINCT";

            _sqlQuery.Replace(SelectionSetTemplate, projectSelectionSet);

            if (!_needProjectCompatibilityFilter)
                _sqlQuery.Replace(distinct, string.Empty);
        }

        /// <inheritdoc/>
        protected override void SetCountSelect()
        {
            var countSelectionTemplate = $"COUNT({CountTemplate})";

            const string count = "*";
            const string projectCompatibilitiesCount = "DISTINCT projectCompatibilities.ProjectId";

            _sqlQuery.Replace(SelectionSetTemplate, countSelectionTemplate);
            _sqlQuery.Replace(TopTemplate, string.Empty);
            _sqlQuery.Replace(OrderByTemplate, string.Empty);
            _sqlQuery.Replace(CountTemplate, _needProjectCompatibilityFilter ? projectCompatibilitiesCount : count);
        }

        /// <inheritdoc/>
        protected override void SetJoin()
        {
            var firstJoin = $@"INNER JOIN ProjectTypes projectType
						       ON projectType.Id = {TableElementName}.TypeId
						       INNER JOIN ProjectCategories projectCategory
						       ON projectCategory.Id = {TableElementName}.CategoryId";


            var secondJoin = $@"INNER JOIN ProjectsCompatibilities projectCompatibilities
								ON projectCompatibilities.ProjectId = {TableElementName}.Id";

            if (TypeOfSelect == SelectTypes.Data)
                _sqlQuery.Replace(FirstJoinTemplate, firstJoin);
            else
                _sqlQuery.Replace(
                    FirstJoinTemplate,
                    GetPartJoin(_projectRequestSettings.FilterValuesDictionary[FilterConstants.ProjectType] != null, _projectRequestSettings.FilterValuesDictionary[FilterConstants.ProjectCategory] != null));

            _sqlQuery.Replace(SecondJoinTemplate, _needProjectCompatibilityFilter ? secondJoin : string.Empty);
        }

        /// <summary>
        /// Получить часть JOIN (для соединения с таблицами типов и категорий проектов)
        /// </summary>
        /// <param name="needJoinWithProjectTypes"> Нужно ли соединение с таблицей типов проектов </param>
        /// <param name="needJoinWithProjectCategories"> Нужно ли соединение с таблицей категорий проектов </param>
        /// <returns> Часть JOIN (для соединения с таблицами типов и категорий проектов) </returns>
        private string GetPartJoin(bool needJoinWithProjectTypes, bool needJoinWithProjectCategories)
        {
            if (needJoinWithProjectTypes && needJoinWithProjectCategories)
                return $@"INNER JOIN ProjectTypes projectType
						  ON projectType.Id = {TableElementName}.TypeId
						  INNER JOIN ProjectCategories projectCategory
						  ON projectCategory.Id = {TableElementName}.CategoryId";

            if (needJoinWithProjectTypes)
                return $@"INNER JOIN ProjectTypes projectType
						  ON projectType.Id = {TableElementName}.TypeId";

            if (needJoinWithProjectCategories)
                return $@"INNER JOIN ProjectCategories projectCategory
						  ON projectCategory.Id = {TableElementName}.CategoryId";

            return string.Empty;
        }

        /// <inheritdoc/>
        protected override void SetWhere()
        {
            const string where = "WHERE ";

            var projectName = _projectRequestSettings.SearchText;
            var filterValuesDictionary = _projectRequestSettings.FilterValuesDictionary;

            if (_offset != -1
                && string.IsNullOrEmpty(projectName)
                && filterValuesDictionary[FilterConstants.ProjectType] is null
                && filterValuesDictionary[FilterConstants.ProjectCategory] is null
                && filterValuesDictionary[FilterConstants.ProjectCompatibility] is null)
            {
                _sqlQuery.Replace(WhereTemplate, string.Empty);
                return;
            }

            var whereBuilder = new StringBuilder(where);

            if (_offset == -1)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.Id], _projectRequestSettings.LastElementId);

            if (string.IsNullOrEmpty(projectName) is false)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.ProjectName], projectName);

            if (filterValuesDictionary[FilterConstants.ProjectType] is not null)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.ProjectType], filterValuesDictionary[FilterConstants.ProjectType]);

            if (filterValuesDictionary[FilterConstants.ProjectCategory] is not null)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.ProjectCategory], filterValuesDictionary[FilterConstants.ProjectCategory]);

            if (filterValuesDictionary[FilterConstants.ProjectCompatibility] is not null)
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.ProjectCompatibility], filterValuesDictionary[FilterConstants.ProjectCompatibility]);

            _sqlQuery.Replace(WhereTemplate, whereBuilder.ToString());
        }

        /// <inheritdoc/>
        protected override void SetOrderBy()
        {
            var needOffset = _offset != -1;
            string orderBy;
            if (needOffset)
            {
                orderBy = $@"ORDER BY {OrderByValueTemplate}
					 OFFSET {OffsetParameter} ROWS
					 FETCH NEXT {_dataCountInPackage} ROWS ONLY";
                _parameters.Add(OffsetParameter, _offset, DbType.Int32, ParameterDirection.Input);
            }
            else
            {
                orderBy = $@"ORDER BY {OrderByValueTemplate}";
            }

            _sqlQuery.Replace(OrderByTemplate, orderBy);
        }

        /// <summary>
        /// Установить сортировку
        /// </summary>
        /// <param name="typeOfSort"> Тип сортировки </param>
        private void SetOrderBy(SortTypes typeOfSort)
        {
            const string standardSort = "project.Id DESC";
            const string countReviewsSort = "CountReviews";
            const string countReviewsSortDesc = "CountReviews DESC";
            const string ratingSort = "AvgRating";
            const string ratingSortDesc = "AvgRating DESC";

            _sqlQuery.Replace(OrderByValueTemplate, typeOfSort switch
            {
                SortTypes.Standard => standardSort,
                SortTypes.CountReviews => countReviewsSort,
                SortTypes.CountReviewsDesc => countReviewsSortDesc,
                SortTypes.Rating => ratingSort,
                SortTypes.RatingDesc => ratingSortDesc,
                _ => throw new System.NotImplementedException()
            });
        }
    }
}