using DapperAssistant.Annotations;

namespace BusinessCard.DataAccessLayer.Entities.Data
{
    /// <summary>
    /// Булевы флаги приложения
    /// </summary>
    [SqlTable("Flags")]
    [NeedInsertId]
    public class Flag
    {
        /// <summary>
        /// Ключ флага
        /// </summary>
        public string FlagKey { get; set; }

        /// <summary>
        /// Значение флага
        /// </summary>
        public bool Value { get; set; }
    }
}