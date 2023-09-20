using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.MAXonService
{
    /// <summary>
    /// Значение условия
    /// </summary>
    [SqlTable("ConditionsValues")]
    public class ConditionValue
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
        /// Идентификатор тарифа
        /// </summary>
        public int RateId { get; set; }

        /// <summary>
        /// Идентификатор условия
        /// </summary>
        [SqlForeignKey("Conditions")]
        public int ConditionId { get; set; }

        /// <summary>
        /// Условие
        /// </summary>
        [RelatedSqlEntity]
        public Condition Condition { get; set; }
    }
}