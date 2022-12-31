using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonBlog;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper
{
    /// <inheritdoc cref="ISelectionQueryBuilderFactory"/>
    public class SelectionQueryBuilderFactory : ISelectionQueryBuilderFactory
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        public SelectionQueryBuilderFactory(IServiceProvider serviceProvider) =>_serviceProvider = serviceProvider;

        public SelectionQueryBuilder GetQueryBuilder(QueryBuilderTypes builderType)
            => builderType switch
            {
                QueryBuilderTypes.Projects => _serviceProvider.GetRequiredService<ProjectsSelectionQueryBuilder>(),

                QueryBuilderTypes.Posts => _serviceProvider.GetRequiredService<PostsSelectionQueryBuilder>(),

                _ => throw new NotImplementedException()
            };
    }
}