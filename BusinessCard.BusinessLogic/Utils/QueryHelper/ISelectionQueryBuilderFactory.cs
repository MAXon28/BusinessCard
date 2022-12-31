namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISelectionQueryBuilderFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builderType">  </param>
        /// <returns>  </returns>
        public SelectionQueryBuilder GetQueryBuilder(QueryBuilderTypes builderType);
    }
}