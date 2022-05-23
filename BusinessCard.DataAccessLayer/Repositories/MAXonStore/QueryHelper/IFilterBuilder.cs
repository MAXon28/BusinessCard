using Dapper;
using System.Text;

namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore.QueryHelper
{
    /// <summary>
    /// Строитель фильтров в SQL-запросе
    /// </summary>
    internal interface IFilterBuilder
    {
        /// <summary>
        /// Установить фильтр
        /// </summary>
        /// <typeparam name="T"> Тип значений фильтрации </typeparam>
        /// <param name="sqlQuery"> SQL-запрос </param>
        /// <param name="parameters"> Параметры для фильтров </param>
        /// <param name="value"> Значения для фильтров </param>
        public void SetFilter<T>(StringBuilder sqlQuery, DynamicParameters parameters, T value);
    }
}