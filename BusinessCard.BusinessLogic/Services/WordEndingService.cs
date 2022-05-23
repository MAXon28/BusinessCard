namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <summary>
    /// Сервис окончания слова (для более правильного вывода слов при подсчёте значений)
    /// </summary>
    internal abstract class WordEndingService
    {
        /// <summary>
        /// 
        /// </summary>
        protected string _firstVariantWord;

        /// <summary>
        /// 
        /// </summary>
        protected string _secondVariantWord;

        /// <summary>
        /// 
        /// </summary>
        protected string _thirdVariantWord;

        /// <summary>
        /// Получить слово с нужным окончанием
        /// </summary>
        /// <param name="number"> Число, по которому определяется окончание </param>
        /// <returns> Слово с нужным окончанием </returns>
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