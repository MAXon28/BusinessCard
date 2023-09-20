using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.Data
{
    /// <summary>
    /// Вычисляемое значение
    /// </summary>
    [SqlTable("CalculatedValues")]
    [NeedInsertId]
    public class CalculatedValue
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Признак того, что значение можно использовать для сервисов
        /// </summary>
        public bool ForService { get; set; }
    }
}