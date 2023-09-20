using System;

namespace BusinessCard.BusinessLogicLayer.Utils.Attributes
{
    /// <summary>
    /// Атрибут настроек обновления
    /// </summary>
    internal class UpdateSettingsAttribute : Attribute
    {
        public UpdateSettingsAttribute(bool needUpdateDetail, bool needUpdateAdditionalData)
        {
            NeedUpdateDetail = needUpdateDetail;
            NeedUpdateAdditionalData = needUpdateAdditionalData;
        }

        /// <summary>
        /// Необходимо обновить детали сущности
        /// </summary>
        public bool NeedUpdateDetail { get; }

        /// <summary>
        /// Необходимо обновить дополнительные данные
        /// </summary>
        public bool NeedUpdateAdditionalData { get; }
    }
}