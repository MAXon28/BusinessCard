using Dapper;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper
{
    /// <summary>
    /// Данные запроса
    /// </summary>
    public class QueryData
    {
        public QueryData(string sqlQuery, DynamicParameters parameters)
        {
            SqlQuery = sqlQuery;
            Parameters = parameters;
        }

        /// <summary>
        /// SQL-запрос
        /// </summary>
        public string SqlQuery { get; }

        /// <summary>
        /// Параметры
        /// </summary>
        public DynamicParameters Parameters { get; }
    }
}