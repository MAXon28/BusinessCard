using System;

namespace BusinessCard.BusinessLogicLayer.Utils.Attributes
{
    /// <summary>
    /// Атрибут вариантов написания слова
    /// </summary>
    internal class WordSpellingsVariantsAttribute : Attribute
    {
        public WordSpellingsVariantsAttribute(string firstVariant, string secondVariant, string thirdVariant)
        {
            FirstVariant = firstVariant;
            SecondVariant = secondVariant;
            ThirdVariant = thirdVariant;
        }

        /// <summary>
        /// Первый вариант слова
        /// </summary>
        public string FirstVariant { get; }

        /// <summary>
        /// Второй вариант слова
        /// </summary>
        public string SecondVariant { get; set; }

        /// <summary>
        /// Третий вариант слова
        /// </summary>
        public string ThirdVariant { get; set; }
    }
}