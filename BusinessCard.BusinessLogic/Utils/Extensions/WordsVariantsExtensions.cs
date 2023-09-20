using BusinessCard.BusinessLogicLayer.Utils.Attributes;
using BusinessCard.BusinessLogicLayer.Utils.Enums;

namespace BusinessCard.BusinessLogicLayer.Utils.Extensions
{
    /// <summary>
    /// Расширения для вариантов написания слов
    /// </summary>
    internal static class WordsVariantsExtensions
    {
        /// <summary>
        /// Получить варианты написания слова
        /// </summary>
        /// <param name="word"> Перечисление </param>
        /// <returns> Варианты написания слова </returns>
        public static (string firstVariant, string secondVariant, string thirdVariant) GetWordSpellingsVariants(this WordsVariants word)
        {
            var fieldInfo = word.GetType().GetField(word.ToString());
            var attributes = (WordSpellingsVariantsAttribute[])fieldInfo.GetCustomAttributes(typeof(WordSpellingsVariantsAttribute), false);
            return attributes.Length > 0
                ? (attributes[0].FirstVariant, attributes[0].SecondVariant, attributes[0].ThirdVariant)
                : (string.Empty, string.Empty, string.Empty);
        }
    }
}