using Dapper;
using System.Data;
using System;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonBlog
{
    /// <inheritdoc cref="IFilterBuilder"/>
    internal class ChannelIdFilterBuilder : IFilterBuilder
    {
        /// <summary>
        /// Параметр значения в запросе
        /// </summary>
        private const string ChannelIdConditionValue = "@ChannelId";

        /// <summary>
        /// Фильтр идентификатора канала
        /// </summary>
        private readonly string _channelIdFilter = $"post.ChannelId = {ChannelIdConditionValue}";

        public void SetFilter<T>(StringBuilder sqlQuery, DynamicParameters parameters, T value)
        {
            if (value is int channelId)
            {
                sqlQuery.Append(_channelIdFilter);
                parameters.Add(ChannelIdConditionValue, channelId, DbType.Int32, ParameterDirection.Input);
            }
            else
            {
                throw new Exception("Значение фильтра неопределено.");
            }
        }
    }
}