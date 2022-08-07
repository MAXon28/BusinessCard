using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore.QueryHelper
{
	/// <summary>
	/// Строитель запроса выборки проектов
	/// </summary>
    internal class ProjectsSelectionQueryBuilder
    {
		/// <summary>
		/// Шаблон выборки данных
		/// </summary>
		private const string SelectionSetTemplate = "*selection_set*";

		/// <summary>
		/// Первая часть шаблона соединения
		/// </summary>
		private const string FirstJoinTemplate = "*join*";

		/// <summary>
		/// Вторая часть шаблона соединения
		/// </summary>
		private const string SecondJoinTemplate = "*join2*";

		/// <summary>
		/// Шаблон фильтрации
		/// </summary>
		private const string WhereTemplate = "*where*";

		/// <summary>
		/// Шаблон сортировки
		/// </summary>
		private const string OrderByTemplate = "*order_by*";

		/// <summary>
		/// Шаблон полей сортировки
		/// </summary>
		private const string OrderByValueTemplate = "*order_by_column type_of_sort*";

		/// <summary>
		/// Шаблон выборки для подсчёта количества
		/// </summary>
		private const string CountTemplate = "*count_element*";

		/// <summary>
		/// Параметр в запросе для сдвига проектов на конкретное число (для пагинации)
		/// </summary>
		private const string OffsetParameter = "@Offset";

		/// <summary>
		/// Шаблон запроса
		/// </summary>
		private readonly string _sqlQueryTemplate = @$"SELECT {SelectionSetTemplate}
												       FROM Projects project 
														{FirstJoinTemplate}
														{SecondJoinTemplate}
												       {WhereTemplate}
												       {OrderByTemplate};";

		/// <summary>
		/// Словарь строителей фильтров
		/// </summary>
		private readonly Dictionary<string, IFilterBuilder> _filterBuildersDictionary = new Dictionary<string, IFilterBuilder>
        {
			[FilterConstants.ProjectName] = new ProjectNameFilterBuilder(),
			[FilterConstants.ProjectType] = new ProjectTypeFilterBuilder(),
			[FilterConstants.ProjectCategory] = new ProjectCategoryFilterBuilder(),
			[FilterConstants.ProjectCompatibility] = new ProjectCompatibilityFilterBuilder()
        };

		/// <summary>
		/// Запрос
		/// </summary>
		private readonly StringBuilder _sqlQuery = new StringBuilder();

		/// <summary>
		/// Параметры запроса
		/// </summary>
		private readonly DynamicParameters _parameters = new DynamicParameters();

		/// <summary>
		/// Получить данные запроса проектов
		/// </summary>
		/// <param name="projectQuerySettings"> Настройки запроса проектов </param>
		/// <param name="typeOfSelect"> Тип выборки </param>
		/// <returns> Данные запроса проектов </returns>
		public ProjectQueryData GetProjectQueryData(ProjectQuerySettings projectQuerySettings, TypeOfSelect typeOfSelect)
        {
			_sqlQuery.Append(_sqlQueryTemplate);

			SetSelect(typeOfSelect, projectQuerySettings.Offset, !(projectQuerySettings.FilterValuesDictionary[FilterConstants.ProjectCompatibility] is null));

			if (typeOfSelect == TypeOfSelect.Projects)
				SetJoin(!(projectQuerySettings.FilterValuesDictionary[FilterConstants.ProjectCompatibility] is null));
			else
				SetJoin(projectQuerySettings.FilterValuesDictionary);

			SetWhere(projectQuerySettings.ProjectName, projectQuerySettings.FilterValuesDictionary);

			if (typeOfSelect == TypeOfSelect.Projects)
				SetOrderBy(projectQuerySettings.TypeOfSort);

			return new ProjectQueryData(_sqlQuery.ToString(), _parameters);
        }

		/// <summary>
		/// Установить выборку
		/// </summary>
		/// <param name="typeOfSelect"> Тип выборки </param>
		/// <param name="offset"> Сдвиг проектов на конкретное число (для пагинации) </param>
		/// <param name="needProjectCompatibilityFilter"> Нужна ли фильтрация по совместимости </param>
		private void SetSelect(TypeOfSelect typeOfSelect, int offset, bool needProjectCompatibilityFilter)
        {
			switch (typeOfSelect)
            {
				case TypeOfSelect.Projects:
					SetProjectSelect(offset, needProjectCompatibilityFilter);
					break;
				case TypeOfSelect.Count:
					SetCountSelect(needProjectCompatibilityFilter);
					break;
			}
        }

		/// <summary>
		/// Установить выборку проектов
		/// </summary>
		/// <param name="offset"> Сдвиг проектов на конкретное число (для пагинации) </param>
		/// <param name="needProjectCompatibilityFilter"> Нужна ли фильтрация по совместимости </param>
		private void SetProjectSelect(int offset, bool needProjectCompatibilityFilter)
        {
			const string projectSelectionSet = @"DISTINCT project.Id,
										                  project.Name,
										                  project.Icon,
										                  projectType.Id,
										                  projectType.Name,
										                  projectCategory.Id,
										                  projectCategory.Name,
														  (SELECT COUNT(*)
														   FROM ProjectReviews projectReview
														   WHERE projectReview.ProjectId = project.id
														   GROUP BY projectReview.ProjectId) AS CountReviews,
										                  (SELECT AVG(CONVERT (float, projectReview.Rating))
											               FROM ProjectReviews projectReview
											               WHERE projectReview.ProjectId = project.id
											               GROUP BY projectReview.ProjectId) AS AvgRating,
										                  STUFF((SELECT CAST(',' AS VARCHAR(MAX)) + projectCompatibility.Name
												                 FROM ProjectCompatibilities projectCompatibility
																	INNER JOIN ProjectsCompatibilities projectCompatibilities
																	ON projectCompatibilities.CompatibilityId = projectCompatibility.Id
												                 WHERE projectCompatibilities.ProjectId = project.Id
												                 FOR XML PATH('')), 1, 1, '') AS ProjectCompatibilities";

			var orderByTemplate = @$"ORDER BY {OrderByValueTemplate}
										OFFSET {OffsetParameter} ROWS
										FETCH NEXT 5 ROWS ONLY";

			const string join = @"INNER JOIN ProjectTypes projectType
								  ON projectType.Id = project.TypeId
								  INNER JOIN ProjectCategories projectCategory
								  ON projectCategory.Id = project.CategoryId";

			const string distinct = "DISTINCT";

			_sqlQuery.Replace(SelectionSetTemplate, projectSelectionSet);
			_sqlQuery.Replace(FirstJoinTemplate, join);
			_sqlQuery.Replace(OrderByTemplate, orderByTemplate);

			_parameters.Add(OffsetParameter, offset, DbType.Int32, ParameterDirection.Input);

			if (!needProjectCompatibilityFilter)
				_sqlQuery.Replace(distinct, string.Empty);
		}

		/// <summary>
		/// Установить выборку количества проектов
		/// </summary>
		/// <param name="needProjectCompatibilityFilter"> Нужна ли фильтрация по совместимости </param>
		private void SetCountSelect(bool needProjectCompatibilityFilter)
        {
			var countSelectionTemplate = $"COUNT({CountTemplate})";

			const string count = "*";
			const string projectCompatibilitiesCount = "DISTINCT projectCompatibilities.ProjectId";

			_sqlQuery.Replace(SelectionSetTemplate, countSelectionTemplate);
			_sqlQuery.Replace(OrderByTemplate, string.Empty);
			_sqlQuery.Replace(CountTemplate, needProjectCompatibilityFilter ? projectCompatibilitiesCount : count);
		}

		/// <summary>
		/// Установить соединение
		/// </summary>
		/// <param name="needJoinWithProjectCompatibilities"> Нужно ли соединение с таблицей совместимостей </param>
		private void SetJoin(bool needJoinWithProjectCompatibilities)
		{
			const string projectCompatibilityJoin = @"INNER JOIN ProjectsCompatibilities projectCompatibilities
													  ON projectCompatibilities.ProjectId = project.Id";

			_sqlQuery.Replace(SecondJoinTemplate, needJoinWithProjectCompatibilities ? projectCompatibilityJoin : string.Empty);
		}

		/// <summary>
		/// Установить соединение
		/// </summary>
		/// <param name="filterValuesDictionary"> Значения фильтров, которые указывают на необходимость соединения с соответствующими таблицами </param>
		private void SetJoin(Dictionary<string, List<int>> filterValuesDictionary)
        {
			const string projectCompatibilityJoin = @"INNER JOIN ProjectsCompatibilities projectCompatibilities
													  ON projectCompatibilities.ProjectId = project.Id";

			_sqlQuery.Replace(FirstJoinTemplate, GetPartJoin(filterValuesDictionary[FilterConstants.ProjectType] != null, filterValuesDictionary[FilterConstants.ProjectCategory] != null));

			_sqlQuery.Replace(SecondJoinTemplate, !(filterValuesDictionary[FilterConstants.ProjectCompatibility] is null) ? projectCompatibilityJoin : string.Empty);
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
				return @"INNER JOIN ProjectTypes projectType
						 ON projectType.Id = project.TypeId
						 INNER JOIN ProjectCategories projectCategory
						 ON projectCategory.Id = project.CategoryId";

			if (needJoinWithProjectTypes)
				return @"INNER JOIN ProjectTypes projectType
						 ON projectType.Id = project.TypeId";

			if (needJoinWithProjectCategories)
				return @"INNER JOIN ProjectCategories projectCategory
						 ON projectCategory.Id = project.CategoryId";

			return string.Empty;
		}

		/// <summary>
		/// Установить фильтрацию
		/// </summary>
		/// <param name="projectName"> Название или часть названия проекта </param>
		/// <param name="filterValuesDictionary"> Словарь значений для выборки </param>
		private void SetWhere(string projectName, Dictionary<string, List<int>> filterValuesDictionary)
        {
			const string where = "WHERE ";

			if (projectName is null && 
				filterValuesDictionary[FilterConstants.ProjectType] is null && 
				filterValuesDictionary[FilterConstants.ProjectCategory] is null && 
				filterValuesDictionary[FilterConstants.ProjectCompatibility] is null)
            {
				_sqlQuery.Replace(WhereTemplate, string.Empty);
            }
			else
            {
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
		}

		/// <summary>
		/// Установить фильтр
		/// </summary>
		/// <param name="whereBuilder"> Собираемая строка фильтрации </param>
		/// <param name="filterBuilder"> Строитель фильтра </param>
		/// <param name="filterValues"> Значения для фильтрации </param>
		private void SetFilter(StringBuilder whereBuilder, IFilterBuilder filterBuilder, List<int> filterValues)
        {
			const string where = "WHERE ";
			const string and = " AND ";

			whereBuilder.Append(whereBuilder.ToString() != where ? and : string.Empty);
			filterBuilder.SetFilter(whereBuilder, _parameters, filterValues);
		}

		/// <summary>
		/// Установить сортировку
		/// </summary>
		/// <param name="typeOfSort"> Тип сортировки </param>
		private void SetOrderBy(TypeOfSort typeOfSort)
        {
			const string standardSort = "project.Id DESC";
			const string countReviewsSort = "CountReviews";
			const string countReviewsSortDesc = "CountReviews DESC";
			const string ratingSort = "AvgRating";
			const string ratingSortDesc = "AvgRating DESC";

			_sqlQuery.Replace(OrderByValueTemplate, typeOfSort switch
            {
                TypeOfSort.Standard => standardSort,
                TypeOfSort.CountReviews => countReviewsSort,
                TypeOfSort.CountReviewsDesc => countReviewsSortDesc,
                TypeOfSort.Rating => ratingSort,
                TypeOfSort.RatingDesc => ratingSortDesc,
                _ => throw new System.NotImplementedException()
            });
        }
	}
}