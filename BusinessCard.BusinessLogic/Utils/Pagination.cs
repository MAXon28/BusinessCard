using BusinessCard.BusinessLogicLayer.Interfaces.Utils;

namespace BusinessCard.BusinessLogicLayer.Utils
{
    /// <inheritdoc cref="IPagination"/>
    internal class Pagination : IPagination
    {
        /// <inheritdoc/>
        public int GetOffset(int countInPackage, int packageNumber)
            => (packageNumber - 1) * countInPackage;

        /// <inheritdoc/>
        public int GetPagesCount(int countInPackage, int elementsCount)
            => elementsCount / countInPackage + (elementsCount % countInPackage > 0 ? 1 : 0);
    }
}