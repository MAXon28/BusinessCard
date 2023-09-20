using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using Dapper;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore
{
    /// <inheritdoc cref="IFilterBuilder"/>
    internal class ProjectNameFilterBuilder : IFilterBuilder
    {
        /// <summary>
        /// Параметр значения в запросе
        /// </summary>
        private const string ProjectNameConditionValue = "@ProjectNamePart";

        /// <summary>
        /// Фильтр названия проекта
        /// </summary>
        private readonly string _projectNameFilter = $"project.Name LIKE '%' + {ProjectNameConditionValue} + '%'";

        public void SetFilter<T>(StringBuilder sqlQuery, DynamicParameters parameters, T value)
        {
            var projectName = value as string;
            sqlQuery.Append(_projectNameFilter);
            parameters.Add(ProjectNameConditionValue, projectName, DbType.String, ParameterDirection.Input);
        }
    }
}