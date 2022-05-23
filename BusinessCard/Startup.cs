using System;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Services;
using DapperAssistant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using BusinessCard.DataAccessLayer.Repositories.Content;
using BusinessCard.DataAccessLayer.Repositories.Data;
using BusinessCard.DataAccessLayer.Interfaces.Data;
using BusinessCard.DataAccessLayer.Interfaces.Content.Services;
using BusinessCard.DataAccessLayer.Repositories.Content.Services;
using BusinessCard.Services;
using BusinessCard.DataAccessLayer.Interfaces.MAXonStore;
using BusinessCard.DataAccessLayer.Repositories.MAXonStore;

namespace BusinessCard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration = builder.Build();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddScoped(dbConnectionKeeper => new DbConnectionKeeper(connection));

            //services.AddScoped(typeof(IRepository<>), typeof(StandardRepository<>));

            services.AddScoped<IFactOnBusinessCardRepository, FactOnBusinessCardRepository>();
            services.AddScoped<IBiographyRepository, BiographyRepository>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IExperienceRepository, ExperienceRepsoitory>();
            services.AddScoped<IEducationRepository, EducationRepository>();
            services.AddScoped<IWorkRepository, WorkRepository>();
            services.AddScoped<IVacancyRepository, VacancyRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IShortDescriptionRepository, ShortDescriptionRepository>();
            services.AddScoped<IRateRepository, RateRepository>();
            services.AddScoped<IConditionRepository, ConditionRepository>();
            services.AddScoped<IConditionValueRepository, ConditionValueRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IRuleRepository, RuleRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectTypeRepository, ProjectTypeRepository>();
            services.AddScoped<IProjectCategoryRepository, ProjectCategoryRepository>();
            services.AddScoped<IProjectCompatibilityRepository, ProjectCompatibilityRepository>();

            services.AddScoped<IBusinessCardService, BusinessCardService>();
            services.AddScoped<IAboutMeService, AboutMeService>();
            services.AddScoped<IWorkService, WorkService>();
            services.AddScoped<ISelfEmployedService, SelfEmployedService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IRuleService, RuleService>();
            services.AddScoped<IStoreService, StoreService>();

            services.AddScoped<FileSaver>();

            services.AddMvc();

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 44364;
            });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("us.example.com");
                options.ExcludedHosts.Add("www.example.com");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=MAXonBusinessCard}/{action=Card}/{id?}");
            });
        }
    }
}