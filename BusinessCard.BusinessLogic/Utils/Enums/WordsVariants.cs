using BusinessCard.BusinessLogicLayer.Utils.Attributes;

namespace BusinessCard.BusinessLogicLayer.Utils.Enums
{
    /// <summary>
    /// Варианты слов
    /// </summary>
    internal enum WordsVariants
    {
        /// <summary>
        /// Проект
        /// </summary>
        [WordSpellingsVariants("Проект", "Проекта", "Проектов")]
        Project = 1,

        /// <summary>
        /// Скачивание
        /// </summary>
        [WordSpellingsVariants("Скачивание", "Скачивания", "Скачиваний")]
        Download = 2,

        /// <summary>
        /// Консультация
        /// </summary>
        [WordSpellingsVariants("Консультация", "Консультации", "Консультаций")]
        Consultation = 3
    }
}