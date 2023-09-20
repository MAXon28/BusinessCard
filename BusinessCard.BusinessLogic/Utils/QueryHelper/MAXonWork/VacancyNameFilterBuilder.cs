using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using Dapper;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonWork
{
    /// <summary>
    /// Строитель фильтра по наименованию вакансии
    /// </summary>
    internal class VacancyNameFilterBuilder : IFilterBuilder
    {
        /// <summary>
        /// Параметр значения в запросе
        /// </summary>
        private const string VacancyNameConditionValue = "@Name";

        /// <summary>
        /// Фильтр номера задачи
        /// </summary>
        private readonly string _vacancyNameFilter = $"vacancy.Name LIKE '%' + {VacancyNameConditionValue} + '%'";

        /// <inheritdoc/>
        public void SetFilter<T>(StringBuilder sqlQuery, DynamicParameters parameters, T value)
        {
            var taskNumber = value as string;
            sqlQuery.Append(_vacancyNameFilter);
            parameters.Add(VacancyNameConditionValue, taskNumber, DbType.String, ParameterDirection.Input);
        }
    }
}