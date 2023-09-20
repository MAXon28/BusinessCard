using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using Dapper;
using System;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonService
{
    /// <summary>
    /// Строитель фильтра статуса задачи
    /// </summary>
    internal class StatusCodeFilterBuilder : IFilterBuilder
    {
        /// <summary>
        /// Параметр значения в запросе
        /// </summary>
        private const string StatusCodeConditionValue = "@StatusCode";

        /// <summary>
        /// Фильтр статуса задачи
        /// </summary>
        private readonly string _statusCodeFilter = $"taskStatus.StatusCode = {StatusCodeConditionValue}";

        public void SetFilter<T>(StringBuilder sqlQuery, DynamicParameters parameters, T value)
        {
            if (value is int statusCode)
            {
                sqlQuery.Append(_statusCodeFilter);
                parameters.Add(StatusCodeConditionValue, statusCode, DbType.Int32, ParameterDirection.Input);
            }
            else
            {
                throw new Exception("Значение фильтра не определено.");
            }
        }
    }
}