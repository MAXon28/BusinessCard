using BusinessCard.BusinessLogicLayer.Utils.Attributes;
using BusinessCard.BusinessLogicLayer.Utils.Enums;

namespace BusinessCard.BusinessLogicLayer.Utils.Extensions
{
    /// <summary>
    /// Расширение для типов обновления
    /// </summary>
    internal static class UpdateTypesExtensions
    {
        /// <summary>
        /// Получить настройки для обновления
        /// </summary>
        /// <param name="updateType"> Перечисление </param>
        /// <returns> Необходимо ли обновлять детали и дополнительную информацию </returns>
        public static (bool needUpdateDetail, bool needUpdateAdditionalData) GetSettingsForUpdate(this UpdateTypes updateType)
        {
            var fieldInfo = updateType.GetType().GetField(updateType.ToString());
            var attributes = (UpdateSettingsAttribute[])fieldInfo.GetCustomAttributes(typeof(UpdateSettingsAttribute), false);
            return attributes.Length > 0
                ? (attributes[0].NeedUpdateDetail, attributes[0].NeedUpdateAdditionalData)
                : (false, false);
        }
    }
}