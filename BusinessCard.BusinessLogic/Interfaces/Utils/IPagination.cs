namespace BusinessCard.BusinessLogicLayer.Interfaces.Utils
{
    /// <summary>
    /// Утилита для пагинации
    /// </summary>
    public interface IPagination
    {
        /// <summary>
        /// Получить сдвиг на конкретное число
        /// </summary>
        /// <param name="countInPackage"> Количество определённых данных в одном пакете </param>
        /// <param name="packageNumber"> Номер пакета </param>
        /// <returns> Сдвиг на конкретное число </returns>
        public int GetOffset(int countInPackage, int packageNumber);

        /// <summary>
        /// Получить количество страниц с элементами
        /// </summary>
        /// <param name="countInPackage"> Количество определённых данных в одном пакете </param>
        /// <param name="elementsCount"> Количество элементов </param>
        /// <returns> Количество страниц с элементами </returns>
        public int GetPagesCount(int countInPackage, int elementsCount);
    }
}