using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore
{
    /// <summary>
    /// Строитель запроса выборки проектов
    /// </summary>
    public class ProjectsSelectionQueryBuilder : SelectionQueryBuilder
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
                [FilterConstants.ProjectName] = new ProjectNameFilterBuilder(),
                [FilterConstants.ProjectType] = new ProjectTypeFilterBuilder(),
                [FilterConstants.ProjectCategory] = new ProjectCategoryFilterBuilder(),
                [FilterConstants.ProjectCompatibility] = new ProjectCompatibilityFilterBuilder()
            };
        }

        /// <inheritdoc/>
        public override QueryData GetQueryData(IRequestSettings requestSettings)
        {
            _projectRequestSettings = requestSettings as ProjectRequestSettings;

            _needProjectCompatibilityFilter = !(_projectRequestSettings.FilterValuesDictionary[FilterConstants.ProjectCompatibility] is null);

            _sqlQuery = new StringBuilder();
            _sqlQuery.Append(_sqlQueryTemplate);

            SetTableNamings(TableName, TableElementName);
            SetSelect(_projectRequestSettings.Offset);
            SetJoin();
            SetWhere();
            if (TypeOfSelect == SelectTypes.Data)
                SetOrderBy(_projectRequestSettings.TypeOfSort);

            return new QueryData(_sqlQuery.ToString(), _parameters);
        }

        /// <summary>
        /// Установить выборку проектов
        /// </summary>
        /// <param name="offset"> Сдвиг проектов на конкретное число (для пагинации) </param>
        protected override void SetDataSelect(int offset)
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

            var orderByTemplate = @$"ORDER BY {OrderByValueTemplate}
										OFFSET {OffsetParameter} ROWS
										FETCH NEXT 5 ROWS ONLY";

            var join = $@"INNER JOIN ProjectTypes projectType
						  ON projectType.Id = {TableElementName}.TypeId
						  INNER JOIN ProjectCategories projectCategory
						  ON projectCategory.Id = {TableElementName}.CategoryId";

            const string distinct = "DISTINCT";

            _sqlQuery.Replace(SelectionSetTemplate, projectSelectionSet);
            _sqlQuery.Replace(FirstJoinTemplate, join);
            _sqlQuery.Replace(OrderByTemplate, orderByTemplate);

            _parameters.Add(OffsetParameter, offset, DbType.Int32, ParameterDirection.Input);

            if (!_needProjectCompatibilityFilter)
                _sqlQuery.Replace(distinct, string.Empty);
        }

        /// <summary>
        /// Установить выборку количества проектов
        /// </summary>
        protected override void SetCountSelect()
        {
            var countSelectionTemplate = $"COUNT({CountTemplate})";

            const string count = "*";
            const string projectCompatibilitiesCount = "DISTINCT projectCompatibilities.ProjectId";

            _sqlQuery.Replace(SelectionSetTemplate, countSelectionTemplate);
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

            if (string.IsNullOrEmpty(projectName) &&
                filterValuesDictionary[FilterConstants.ProjectType] is null &&
                filterValuesDictionary[FilterConstants.ProjectCategory] is null &&
                filterValuesDictionary[FilterConstants.ProjectCompatibility] is null)
            {
                _sqlQuery.Replace(WhereTemplate, string.Empty);
                return;
            }

            var whereBuilder = new StringBuilder(where);

            if (!(projectName is null))
                _filterBuildersDictionary[FilterConstants.ProjectName].SetFilter(whereBuilder, _parameters, projectName);

            if (!(filterValuesDictionary[FilterConstants.ProjectType] is null))
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.ProjectType], filterValuesDictionary[FilterConstants.ProjectType]);

            if (!(filterValuesDictionary[FilterConstants.ProjectCategory] is null))
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.ProjectCategory], filterValuesDictionary[FilterConstants.ProjectCategory]);

            if (!(filterValuesDictionary[FilterConstants.ProjectCompatibility] is null))
                SetFilter(whereBuilder, _filterBuildersDictionary[FilterConstants.ProjectCompatibility], filterValuesDictionary[FilterConstants.ProjectCompatibility]);

            _sqlQuery.Replace(WhereTemplate, whereBuilder.ToString());
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