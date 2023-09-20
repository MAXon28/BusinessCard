namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonWork
{
    /// <summary>
    /// 
    /// </summary>
    internal class VacancyRequestSettings : RequestSettings
    {
        public VacancyRequestSettings(int lastVacancyId, int vacanciesCountInPackage, string searchText) : base(lastVacancyId, vacanciesCountInPackage, searchText) { }
    }
}