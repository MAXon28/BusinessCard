namespace BusinessCard.BusinessLogicLayer.Utils
{
    /// <summary>
    /// Утилита для для пагинации
    /// </summary>
    internal class PaginationtUtil
    {
        /// <summary>
        /// Количество определённых данных в одном пакете
        /// </summary>
        private readonly int _countInPackage;

        public PaginationtUtil(int countInPackage) => _countInPackage = countInPackage;

        /// <summary>
        /// Получить сдвиг на конкретное число
        /// </summary>
        /// <param name="packageNumber"> Номер пакета </param>
        /// <returns> Сдвиг на конкретное число </returns>
        public int GetOffset(int packageNumber) => (packageNumber - 1) * _countInPackage;

        /// <summary>
        /// Получить количество страниц с элементами
        /// </summary>
        /// <param name="elementsCount"> Количество элементов </param>
        /// <returns> Количество страниц с элементами </returns>
        public int GetPagesCount(int elementsCount) => elementsCount / _countInPackage + (elementsCount % _countInPackage > 0 ? 1 : 0);
    }
}