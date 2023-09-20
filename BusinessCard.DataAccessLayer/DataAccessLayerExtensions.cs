using BusinessCard.DataAccessLayer.Interfaces.Content;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using BusinessCard.DataAccessLayer.Interfaces.MAXon28Team;
using BusinessCard.DataAccessLayer.Interfaces.MAXonBlog;
using BusinessCard.DataAccessLayer.Interfaces.MAXonService;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using BusinessCard.DataAccessLayer.Repositories.Content;
using BusinessCard.DataAccessLayer.Repositories.Data;
using BusinessCard.DataAccessLayer.Repositories.MAXon28Team;
using BusinessCard.DataAccessLayer.Repositories.MAXonBlog;
using BusinessCard.DataAccessLayer.Repositories.MAXonService;
using BusinessCard.DataAccessLayer.Repositories.MAXonStore;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessCard.DataAccessLayer
{
    /// <summary>
    /// Расширение для добавления сервисов работы с данными приложения
    /// </summary>
    public static class DataAccessLayerExtensions
    {
        /// <summary>
        /// Добавить сервисы для работы с данными приложения
        /// </summary>
        /// <param name="services"> Сервисы </param>
        /// <returns> Сервисы </returns>
        public static IServiceCollection AddDataAccessLayerServices(this IServiceCollection services)
            => services
                .AddScoped<IFactOnBusinessCardRepository, FactOnBusinessCardRepository>()
                .AddScoped<IBiographyRepository, BiographyRepository>()
                .AddScoped<ISkillRepository, SkillRepository>()
                .AddScoped<IExperienceRepository, ExperienceRepsoitory>()
                .AddScoped<IEducationRepository, EducationRepository>()
                .AddScoped<IWorkRepository, WorkRepository>()
                .AddScoped<IVacancyRepository, VacancyRepository>()
                .AddScoped<IFlagRepository, FlagRepository>()
                .AddScoped<IServiceRepository, ServiceRepository>()
                .AddScoped<IShortDescriptionRepository, ShortDescriptionRepository>()
                .AddScoped<IRateRepository, RateRepository>()
                .AddScoped<IServiceCalculatedValueRepository, ServiceCalculatedValueRepository>()
                .AddScoped<IConditionRepository, ConditionRepository>()
                .AddScoped<IConditionValueRepository, ConditionValueRepository>()
                .AddScoped<ICalculatedValueRepository, CalculatedValueRepository>()
                .AddScoped<ITaskRepository, TaskRepository>()
                .AddScoped<ITaskPersonalInfoRepository, TaskPersonalInfoRepository>()
                .AddScoped<ITaskCounterRepository, TaskCounterRepository>()
                .AddScoped<ITaskRecordRepository, TaskRecordRepository>()
                .AddScoped<ITaskReviewRepository, TaskReviewRepository>()
                .AddScoped<ITaskStatusRepository, TaskStatusRepository>()
                .AddScoped<IRuleRepository, RuleRepository>()
                .AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<IProjectTypeRepository, ProjectTypeRepository>()
                .AddScoped<IProjectCategoryRepository, ProjectCategoryRepository>()
                .AddScoped<IProjectCompatibilityRepository, ProjectCompatibilityRepository>()
                .AddScoped<IProjectImageRepository, ProjectImageRepository>()
                .AddScoped<IProjectReviewRepository, ProjectReviewRepository>()
                .AddScoped<IProjectTechnicalRequirementValueRepository, ProjectTechnicalRequirementValueRepository>()
                .AddScoped<IChannelRepository, ChannelRepository>()
                .AddScoped<IPostRepository, PostRepository>()
                .AddScoped<IUserStatisticRepository, UserStatisticRepository>()
                .AddScoped<ITopchikRepository, TopchikRepository>()
                .AddScoped<IBookmarkRepository, BookmarkRepository>()
                .AddScoped<IChannelSubscriptionRepository, ChannelSubscriptionRepository>()
                .AddScoped<IMailingSubscriptionRepository, MailingSubscriptionRepository>()
                .AddScoped<IPostFieldRepository, PostFieldRepository>()
                .AddScoped<IPostElementRepository, PostElementRepository>()
                .AddScoped<ICommentBranchRepository, CommentBranchRepository>()
                .AddScoped<ICommentRepository, CommentRepository>()
                .AddScoped<IUserRepository, UserRepository>();
    }
}