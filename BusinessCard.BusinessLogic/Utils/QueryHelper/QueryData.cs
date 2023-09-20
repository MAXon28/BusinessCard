using Dapper;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper
{
    /// <summary>
    /// Данные запроса
    /// </summary>
    /// <param name="SqlQuery"> SQL-запрос </param>
    /// <param name="Parameters"> Параметры </param>
    public record QueryData(string SqlQuery, DynamicParameters Parameters);
}