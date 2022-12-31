using Dapper;
using System;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonBlog
{
    /// <inheritdoc cref="IFilterBuilder"/>
    internal class UserIdFilterBuilder : IFilterBuilder
    {
        /// <summary>
        /// Параметр значения в запросе
        /// </summary>
        private const string UserIdConditionValue = "@UserId";

        /// <summary>
        /// Фильтр идентификатора пользователя
        /// </summary>
        private readonly string _userIdFilter = $"bookmark.UserId = {UserIdConditionValue}";

        public void SetFilter<T>(StringBuilder sqlQuery, DynamicParameters parameters, T value)
        {
            if (value is int userId)
            {
                sqlQuery.Append(_userIdFilter);
                parameters.Add(UserIdConditionValue, userId, DbType.Int32, ParameterDirection.Input);
            }
            else
            {
                throw new Exception("Значение фильтра неопределено.");
            }
        }
    }
}