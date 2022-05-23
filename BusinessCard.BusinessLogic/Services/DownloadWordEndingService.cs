namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="WordEndingService"/>
    internal class DownloadWordEndingService : WordEndingService
    {
        public DownloadWordEndingService()
        {
            _firstVariantWord = "Скачивание";
            _secondVariantWord = "Скачивания";
            _thirdVariantWord = "Скачиваний";
        }
    }
}