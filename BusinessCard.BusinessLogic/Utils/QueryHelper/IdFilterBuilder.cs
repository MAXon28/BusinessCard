using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using Dapper;
using System;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper
{
    /// <summary>
    /// Строитель фильтра идентификатора в заданной таблице
    /// </summary>
    internal class IdFilterBuilder : IFilterBuilder
    {
        /// <summary>
        /// Параметр значения в запросе
        /// </summary>
        private const string IdConditionValue = "@Id";

        /// <summary>
        /// Фильтр идентификатора
        /// </summary>
        private readonly string _idFilter;

        public IdFilterBuilder(string tableElementName) => _idFilter = $"{tableElementName}.Id < {IdConditionValue}";

        public void SetFilter<T>(StringBuilder sqlQuery, DynamicParameters parameters, T value)
        {
            if (value is int id)
            {
                sqlQuery.Append(_idFilter);
                parameters.Add(IdConditionValue, id, DbType.Int32, ParameterDirection.Input);
            }
            else
            {
                throw new Exception("Значение фильтра не определено.");
            }
        }
    }
}
