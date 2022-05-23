namespace BusinessCard.DataAccessLayer.Repositories.MAXonStore.QueryHelper
{
    /// <summary>
    /// Тип запроса выборки (в зависимости от значений запрос будет выдавать соответсвующие данные)
    /// </summary>
    public enum TypeOfSelect
    {
        /// <summary>
        /// Выборка проектов
        /// </summary>
        Projects,

        /// <summary>
        /// Выборка количества данных в таблице
        /// </summary>
        Count
    }
}