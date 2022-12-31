namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper
{
    /// <summary>
    /// Тип запроса выборки (в зависимости от значений запрос будет выдавать соответсвующие данные)
    /// </summary>
    public enum SelectTypes
    {
        /// <summary>
        /// Выборка данных
        /// </summary>
        Data,

        /// <summary>
        /// Выборка количества данных в таблице
        /// </summary>
        Count
    }
}