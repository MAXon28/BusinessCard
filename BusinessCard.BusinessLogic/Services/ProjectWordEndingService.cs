using BusinessCard.BusinessLogicLayer.Interfaces;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="WordEndingService"/>
    public class ProjectWordEndingService : WordEndingService
    {
        public ProjectWordEndingService() : base()
        {
            _firstVariantWord = "Проект";
            _secondVariantWord = "Проекта";
            _thirdVariantWord = "Проектов";
        }
    }
}