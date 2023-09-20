using BusinessCard.BusinessLogicLayer.Interfaces.Utils;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;

namespace BusinessCard.BusinessLogicLayer.Utils
{
    /// <inheritdoc cref="IWordEnding"/>
    internal class WordEnding : IWordEnding
    {
        /// <summary>
        /// Первый вариант написания слова
        /// </summary>
        private string _firstVariantWord;

        /// <summary>
        /// Второй вариант написания слова
        /// </summary>
        private string _secondVariantWord;

        /// <summary>
        /// Третий вариант написания слова
        /// </summary>
        private string _thirdVariantWord;

        /// <inheritdoc/>
        public void Init(WordsVariants word)
            => (_firstVariantWord, _secondVariantWord, _thirdVariantWord) = word.GetWordSpellingsVariants();

        /// <inheritdoc/>
        public string GetWord(int number)
        {
            var operatingValue = number % 10;

            if (operatingValue == 1 && number % 100 != 11)
                return _firstVariantWord;

            if (operatingValue >= 2 && operatingValue <= 4 && (number % 100 < 12 || number % 100 > 14))
                return _secondVariantWord;

            return _thirdVariantWord;
        }
    }
}