using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore
{
    /// <inheritdoc cref="IFilterBuilder"/>
    internal class ProjectTypeFilterBuilder : IFilterBuilder
    {
        /// <summary>
        /// Поле условия фильтрации
        /// </summary>
        private const string ProjectTypeConditionName = "projectType.Id";

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
        private const string ProjectTypeConditionValue = "@ProjectTypeId";

        /// <summary>
        /// Первый вариант типа условия
        /// </summary>
        private readonly string _firstConditionType = $"= {ProjectTypeConditionValue}";

        /// <summary>
        /// Второй вариант типа условия
        /// </summary>
        private readonly string _secondConditionType = $"IN ({ConditionValueTemplate})";

        /// <summary>
        /// Шаблон фильтра
        /// </summary>
        private readonly string _projectTypeFilterTempalte = $"{ProjectTypeConditionName} {ConditionTypeTemplate}";

        public void SetFilter<T>(StringBuilder sqlQuery, DynamicParameters parameters, T value)
        {
            sqlQuery.Append(_projectTypeFilterTempalte);

            var projectTypesId = value as List<int>;

            if (projectTypesId.Count == 1)
            {
                sqlQuery.Replace(ConditionTypeTemplate, _firstConditionType);
                parameters.Add(ProjectTypeConditionValue, projectTypesId[0], DbType.Int32, ParameterDirection.Input);
            }
            else
            {
                const string separator = ",";

                sqlQuery.Replace(ConditionTypeTemplate, _secondConditionType);

                var conditionValues = new StringBuilder();
                for (var i = 0; i < projectTypesId.Count; i++)
                {
                    var projectTypeConditionValue = ProjectTypeConditionValue + (i + 1);
                    conditionValues.Append(projectTypeConditionValue);
                    parameters.Add(projectTypeConditionValue, projectTypesId[i], DbType.Int32, ParameterDirection.Input);

                    if (i + 1 != projectTypesId.Count)
                        conditionValues.Append(separator);
                }

                sqlQuery.Replace(ConditionValueTemplate, conditionValues.ToString());
            }
        }
    }
}