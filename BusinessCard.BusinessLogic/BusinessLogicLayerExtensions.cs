using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Interfaces.Utils;
using BusinessCard.BusinessLogicLayer.Interfaces.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Interfaces.Utils.TasksRecords;
using BusinessCard.BusinessLogicLayer.Services;
using BusinessCard.BusinessLogicLayer.Utils;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonBlog;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonService;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonStore;
using BusinessCard.BusinessLogicLayer.Utils.QueryHelper.MAXonWork;
using BusinessCard.BusinessLogicLayer.Utils.TasksRecords.Data;
using BusinessCard.BusinessLogicLayer.Utils.TasksRecords.Text;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessCard.BusinessLogicLayer
{
    /// <summary>
    /// Расширение для добавления сервисов работы с бизнес-логикой приложения
    /// </summary>
    public static class BusinessLogicLayerExtensions
    {
        /// <summary>
        /// Добавить сервисы для работы с бизнес-логикой приложения
        /// </summary>
        /// <param name="services"> Сервисы </param>
        /// <returns> Сервисы </returns>
        public static IServiceCollection AddBusinessLogicLayerServices(this IServiceCollection services)
            => services
                .AddScoped<IBusinessCardService, BusinessCardService>()
                .AddScoped<IAboutMeService, AboutMeService>()
                .AddScoped<IWorkService, WorkService>()
                .AddScoped<ISelfEmployedService, SelfEmployedService>()
                .AddScoped<IRateService, RateService>()
                .AddScoped<IConditionService, ConditionService>()
                .AddScoped<ITaskCounterService, TaskCounterService>()
                .AddScoped<ITaskRecordService, TaskRecordService>()
                .AddScoped<ITaskService, TaskService>()
                .AddScoped<ITaskReviewService, TaskReviewService>()
                .AddScoped<ITaskStatusService, TaskStatusService>()
                .AddScoped<IRuleService, RuleService>()
                .AddScoped<IStoreService, StoreService>()
                .AddScoped<IProjectReviewService, ProjectReviewService>()
                .AddScoped<IBlogService, BlogService>()
                .AddScoped<IChannelService, ChannelService>()
                .AddScoped<IPostService, PostService>()
                .AddScoped<IPersonalInformationService, PersonalInformationService>()
                .AddScoped<ICommentService, CommentService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ISelectionQueryBuilderFactory, SelectionQueryBuilderFactory>()
                .AddScoped<ITaskRecordDataFactory, TaskRecordDataFactory>()
                .AddScoped<ITaskRecordTextFactory, TaskRecordTextFactory>()
                .AddScoped<IValidator, Validator>()
                .AddScoped<IPagination, Pagination>()
                .AddScoped<IWordEnding, WordEnding>()
                .AddScoped<ProjectsSelectionQueryBuilder>()
                .AddScoped<PostsSelectionQueryBuilder>()
                .AddScoped<TasksSelectionQueryBuilder>()
                .AddScoped<VacanciesSelectionQueryBuilder>()
                .AddScoped<TaskCreationRecordText>()
                .AddScoped<TaskStatusUpdateRecordData>()
                .AddScoped<TaskPriceAdditionRecordData>()
                .AddScoped<TaskFileAdditionRecordText>()
                .AddScoped<TaskCounterUpdateRecordData>()
                .AddScoped<TaskReviewCreationRecordText>()
                .AddScoped<DownloadDoneTaskFileRecordText>();
    }
}