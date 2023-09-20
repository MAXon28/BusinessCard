using BusinessCard.BusinessLogicLayer.Utils.Enums;

namespace BusinessCard.BusinessLogicLayer.Interfaces.Utils
{
    /// <summary>
    /// Сервис окончания слова (для более правильного вывода слов при подсчёте значений)
    /// </summary>
    internal interface IWordEnding
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="word"> Слово </param>
        public void Init(WordsVariants word);

        /// <summary>
        /// Получить слово с нужным окончанием
        /// </summary>
        /// <param name="number"> Число, по которому определяется окончание </param>
        /// <returns> Слово с нужным окончанием </returns>
        public string GetWord(int number);
    }
}