using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore
{
    /// <inheritdoc cref="IFilterBuilder"/>
    internal class ProjectCompatibilityFilterBuilder : IFilterBuilder
    {
        /// <summary>
        /// Поле условия фильтрации
        /// </summary>
        private const string ProjectCompatibilityConditionName = "projectCompatibilities.CompatibilityId";

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
        private const string ProjectCompatibilityConditionValue = "@ProjectCompatibilityId";

        /// <summary>
        /// Первый вариант типа условия
        /// </summary>
        private readonly string _firstConditionType = $"= {ProjectCompatibilityConditionValue}";

        /// <summary>
        /// Второй вариант типа условия
        /// </summary>
        private readonly string _secondConditionType = $"IN ({ConditionValueTemplate})";

        /// <summary>
        /// Шаблон фильтра
        /// </summary>
        private readonly string _projectCompatibilityFilterTempalte = $"{ProjectCompatibilityConditionName} {ConditionTypeTemplate}";

        public void SetFilter<T>(StringBuilder sqlQuery, DynamicParameters parameters, T value)
        {
            sqlQuery.Append(_projectCompatibilityFilterTempalte);

            var projectCompatibilitiesId = value as List<int>;

            if (projectCompatibilitiesId.Count == 1)
            {
                sqlQuery.Replace(ConditionTypeTemplate, _firstConditionType);
                parameters.Add(ProjectCompatibilityConditionValue, projectCompatibilitiesId[0], DbType.Int32, ParameterDirection.Input);
            }
            else
            {
                const char separator = ',';

                sqlQuery.Replace(ConditionTypeTemplate, _secondConditionType);

                var conditionValues = new StringBuilder();
                for (var i = 0; i < projectCompatibilitiesId.Count; i++)
                {
                    var projectCompatibilityConditionValue = ProjectCompatibilityConditionValue + (i + 1);
                    conditionValues.Append(projectCompatibilityConditionValue);
                    parameters.Add(projectCompatibilityConditionValue, projectCompatibilitiesId[i], DbType.Int32, ParameterDirection.Input);

                    if (i + 1 != projectCompatibilitiesId.Count)
                        conditionValues.Append(separator);
                }

                sqlQuery.Replace(ConditionValueTemplate, conditionValues.ToString());
            }
        }
    }
}