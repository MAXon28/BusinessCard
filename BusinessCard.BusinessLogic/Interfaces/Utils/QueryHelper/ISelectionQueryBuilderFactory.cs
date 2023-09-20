using BusinessCard.BusinessLogicLayer.Utils.Enums;

namespace BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper
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
        public ISelectionQueryBuilder GetQueryBuilder(QueryBuilderTypes builderType);
    }
}