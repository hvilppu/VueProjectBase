using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using project.Hubs;
using project.Services;
using project.Web;
using project.Web.Swagger;
using Project.Api;
using Project.Common;
using Project.Common.Options;
using Project.Domain.Models;
using Project.Domain.Repositories;
using Project.Domain.Services;
using Projects.Hubs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Project
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration Configuration;
        private readonly IHostEnvironment Environment;
        private IServiceCollection currentServices;
        private readonly string _onnxModelFilePath;
        private readonly string _mlnetModelFilePath;
        /// <summary>
        /// Application container
        /// </summary>
        public IContainer ApplicationContainer { get; private set; }

        /// <summary>
        /// Startup for back end
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="configuration"></param>
        public Startup(IHostEnvironment environment, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(environment.ContentRootPath)
#if RELEASE
            .AddJsonFile($"appsettings.Live.json", optional: true)
#elif DEBUG
            .AddJsonFile($"appsettings.Development.json", optional: true)
#endif
    .AddEnvironmentVariables();
            this.Configuration = builder.Build();

            //Configuration = configuration;
            Environment = environment;

            //_onnxModelFilePath = CommonHelpers.GetAbsolutePath(configuration["MLModel:OnnxModelFilePath"]);
            //_mlnetModelFilePath = CommonHelpers.GetAbsolutePath(configuration["MLModel:MLNETModelFilePath"]);
            //var onnxModelConfigurator = new OnnxModelConfigurator(new TinyYoloModel(_onnxModelFilePath));
            //onnxModelConfigurator.SaveMLNetModel(_mlnetModelFilePath);

        }

        /// <summary>
        /// Services are configured here
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            var defaultLang = Configuration["projectOptions:DefaultLang"];
            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                new CultureInfo("en-US"),
                new CultureInfo("fi-FI"),
            };
                options.DefaultRequestCulture = new RequestCulture(defaultLang);

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting 
                // numbers, dates, etc.

                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, 
                // i.e. we have localized resources for.

                options.SupportedUICultures = supportedCultures;
            });

            //Configuration
            services.Configure<ProjectOptions>(Configuration.GetSection("ProjectOptions"));
            services.Configure<ProjectCommonOptions>(Configuration.GetSection("ProjectCommonOptions"));

            //Automapper
            //services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));

            //Swagger
            services.AddSwaggerGen(SwaggerGenHelper.SetSwaggerGenOptions());

            var connection = Configuration.GetConnectionString("ProjectConnectionString");
            services.AddDbContext<ProjectContext>(options =>
            {
                options.UseSqlServer(connection);
            });

            //Repositories
            services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            //Services 
            services.AddTransient<Project.Common.Services.AuthenticationService, Project.Common.Services.AuthenticationService>();
            services.AddTransient<IUserService, UserService>();


            services.AddTransient<ProjectCommonService, ProjectCommonService>();

            services.AddSingleton<IProjectHub, ProjectHub>();
            services.AddTransient<ProjectDailyJob>();

            services.AddSingleton<IMemoryCache, ProjectMemoryCache>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCors();
            services.AddRazorPages().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSignalR(hubOptions =>
            {
                hubOptions.MaximumReceiveMessageSize = 9999999;


            }).AddNewtonsoftJsonProtocol(
                options =>
                {
                    options.PayloadSerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            services.AddDistributedMemoryCache();

            services.AddMvcCore(c => c.Conventions.Add(new ApiExplorerGroupPerVersionConvention())).AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ContractResolver = new DefaultContractResolver();
                o.SerializerSettings.Formatting = Formatting.Indented;
                o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            }).AddApiExplorer();

            services.AddMemoryCache();

            // AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            // Because for error:
            // System.Text.Json.JsonException: A possible object cycle was detected which is not supported. 
            // This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();


            var permissions = GetConstants(typeof(Project_Permissions));

            services.AddAuthorization(options =>
            {
                foreach (var permission in permissions)
                {
                    options.AddPolicy(permission.Name,
                    policy => policy.RequireClaim("Permissions", permission.Name));
                }
            });


            var builder = new ContainerBuilder();
            builder.Populate(services);
            ApplicationContainer = builder.Build();

            currentServices = services;

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configure app
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var projectContext = app.ApplicationServices.GetService<ProjectContext>();

            app.UseCors("AllowAll");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    EnvParam = new { API = System.Environment.GetEnvironmentVariable("API_ROOT") }
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHsts(); //https
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWebSockets();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ProjectHub>("/hub");
            });

            app.UseSwagger(o => o.SerializeAsV2 = true);
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project API v1");
                c.SwaggerEndpoint("/swagger/v1-checkpoint/swagger.json", "Project API Checkpoint v1");
            });

          

            DatabaseCreator databaseCreator = new DatabaseCreator(projectContext);
            databaseCreator.CreateDatabase();

            ProjectDailyService poolsDailyService = new ProjectDailyService(app.ApplicationServices);
            poolsDailyService.Run();

        }

        private List<FieldInfo> GetConstants(Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public |
            BindingFlags.Static | BindingFlags.FlattenHierarchy);
            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
        }
    }
}
