using BusinessCard.BusinessLogicLayer.Utils.Attributes;

namespace BusinessCard.BusinessLogicLayer.Utils.Enums
{
    /// <summary>
    /// Типы обновлений для сервисов
    /// </summary>
    internal enum UpdateTypes
    {
        /// <summary>
        /// Обновить только детали
        /// </summary>
        [UpdateSettings(true, false)]
        DetailData = 1,

        /// <summary>
        /// Обновить только дополнительные данные
        /// </summary>
        [UpdateSettings(false, true)]
        AdditionalData = 2,

        /// <summary>
        /// Обновить все данные
        /// </summary>
        [UpdateSettings(true, true)]
        All = 3
    }
}