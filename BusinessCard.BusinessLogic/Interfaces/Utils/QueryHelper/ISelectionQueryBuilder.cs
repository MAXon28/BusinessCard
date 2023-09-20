using BusinessCard.BusinessLogicLayer.Utils.QueryHelper;

namespace BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper
{
    /// <summary>
    /// Строитель запросов выборки SQL
    /// </summary>
    public interface ISelectionQueryBuilder
    {
        /// <summary>
        /// Тип выборки
        /// </summary>
        public SelectTypes TypeOfSelect { get; set; }

        /// <summary>
        /// Получить данные для запроса
        /// </summary>
        /// <param name="requestSettings"> Настройки запроса </param>
        /// <returns> Данные для запроса </returns>
        public QueryData GetQueryData(RequestSettings requestSettings);
    }
}