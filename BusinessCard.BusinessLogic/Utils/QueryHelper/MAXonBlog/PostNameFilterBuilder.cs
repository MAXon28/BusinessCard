using Dapper;
using System.Data;
using System.Text;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonBlog
{
    /// <inheritdoc cref="IFilterBuilder"/>
    internal class PostNameFilterBuilder : IFilterBuilder
    {
        /// <summary>
        /// Параметр значения в запросе
        /// </summary>
        private const string PostNameConditionValue = "@PostNamePart";

        /// <summary>
        /// Фильтр названия поста
        /// </summary>
        private readonly string _postNameFilter = $"post.Name LIKE '%' + {PostNameConditionValue} + '%'";

        public void SetFilter<T>(StringBuilder sqlQuery, DynamicParameters parameters, T value)
        {
            var postName = value as string;
            sqlQuery.Append(_postNameFilter);
            parameters.Add(PostNameConditionValue, postName, DbType.String, ParameterDirection.Input);
        }
    }
}