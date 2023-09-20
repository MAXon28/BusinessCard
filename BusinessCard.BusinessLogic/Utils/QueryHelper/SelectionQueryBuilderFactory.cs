using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Utils.Enums;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonBlog;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonService;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonWork;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BusinessCard.BusinessLogicLayer.Utils.QueryHelper
{
    /// <inheritdoc cref="ISelectionQueryBuilderFactory"/>
    internal class SelectionQueryBuilderFactory : ISelectionQueryBuilderFactory
    {
        /// <summary>
        /// Провайдер сервисов
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        public SelectionQueryBuilderFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public ISelectionQueryBuilder GetQueryBuilder(QueryBuilderTypes builderType)
            => builderType switch
            {
                QueryBuilderTypes.Projects => _serviceProvider.GetRequiredService<ProjectsSelectionQueryBuilder>(),

                QueryBuilderTypes.Posts => _serviceProvider.GetRequiredService<PostsSelectionQueryBuilder>(),

                QueryBuilderTypes.Tasks => _serviceProvider.GetRequiredService<TasksSelectionQueryBuilder>(),

                QueryBuilderTypes.Vacancies => _serviceProvider.GetRequiredService<VacanciesSelectionQueryBuilder>(),

                _ => throw new NotImplementedException()
            };
    }
}