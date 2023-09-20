namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RequestSettings
    {
        protected RequestSettings(int lastElementId, int countInPackage, string searchText)
        {
            LastElementId = lastElementId;
            CountInPackage = countInPackage;
            SearchText = searchText;
        }

        /// <summary>
        /// 
        /// </summary>
        public int LastElementId { get; }

        /// <summary>
        /// 
        /// </summary>
        public int CountInPackage { get; }

        /// <summary>
        /// 
        /// </summary>
        public string SearchText { get; }
    }
}