using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore
{
    /// <inheritdoc cref="IFilterBuilder"/>
    internal class ProjectCategoryFilterBuilder : IFilterBuilder
    {
        /// <summary>
        /// Поле условия фильтрации
        /// </summary>
        private const string ProjectCategoryConditionName = "projectCategory.Id";

        /// <summary>
        /// Шаблон типа условия
        /// </summary>
        private const string ConditionTypeTemplate = "*condition_type*";

        /// <summary>
        /// Шаблон значения условия
        /// </summary>
        private const string ConditionValueTemplate = "*condition_value*";

        /// <summary>
        /// Параметр значения в запросе
        /// </summary>
        private const string ProjectCategoryConditionValue = "@ProjectCategoryId";

        /// <summary>
        /// Первый вариант типа условия
        /// </summary>
        private readonly string _firstConditionType = $"= {ProjectCategoryConditionValue}";

        /// <summary>
        /// Второй вариант типа условия
        /// </summary>
        private readonly string _secondConditionType = $"IN ({ConditionValueTemplate})";

        /// <summary>
        /// Шаблон фильтра
        /// </summary>
        private readonly string _projectCategoryFilterTempalte = $"{ProjectCategoryConditionName} {ConditionTypeTemplate}";

        public void SetFilter<T>(StringBuilder sqlQuery, DynamicParameters parameters, T value)
        {
            sqlQuery.Append(_projectCategoryFilterTempalte);

            var projectCategoriesId = value as List<int>;

            if (projectCategoriesId.Count == 1)
            {
                sqlQuery.Replace(ConditionTypeTemplate, _firstConditionType);
                parameters.Add(ProjectCategoryConditionValue, projectCategoriesId[0], DbType.Int32, ParameterDirection.Input);
            }
            else
            {
                const string separator = ",";

                sqlQuery.Replace(ConditionTypeTemplate, _secondConditionType);

                var conditionValues = new StringBuilder();
                for (var i = 0; i < projectCategoriesId.Count; i++)
                {
                    var projectCategoryConditionValue = ProjectCategoryConditionValue + (i + 1);
                    conditionValues.Append(projectCategoryConditionValue);
                    parameters.Add(projectCategoryConditionValue, projectCategoriesId[i], DbType.Int32, ParameterDirection.Input);

                    if (i + 1 != projectCategoriesId.Count)
                        conditionValues.Append(separator);
                }

                sqlQuery.Replace(ConditionValueTemplate, conditionValues.ToString());
            }
        }
    }
}