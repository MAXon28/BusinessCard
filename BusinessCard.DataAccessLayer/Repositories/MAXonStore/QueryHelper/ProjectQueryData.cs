using Dapper;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore.QueryHelper
{
    /// <summary>
    /// Данные запроса проектов
    /// </summary>
    internal class ProjectQueryData
    {
        public ProjectQueryData(string sqlQuery, DynamicParameters parameters)
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