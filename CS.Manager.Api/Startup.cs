using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CS.Manager.Application.Auth;
using CS.Manager.Application.Auth.Interfaces;
using CS.Manager.Infrastructure.Swagger;
using CS.Repository;
using CS.Repository.Core.DbType;
using CS.Repository.Core.Entity;
using CS.Repository.Core.User;
using CSRedis;
using FreeSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using CS.Manager.Infrastructure.Utils;
using CS.Manager.Infrastructure.Filter;
using CS.Manager.Infrastructure.Middlewares;
using CS.Manager.Api.Authorization;
using NLog.Extensions.Logging;
using NLog.Web;
using CS.Manager.EasyNetQ.Configuration;
using CS.Manager.EasyNetQ.Consumer;
using EasyNetQ.Consumer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CS.Manager.Application.RabbitMq.Interfaces;
using CS.Manager.Application.RabbitMq;
using EasyNetQ.DI;
using Hangfire;
using Hangfire.Dashboard;
using CS.Manager.Infrastructure.Jobs.HangfireJobExtensions;
using StackExchange.Redis;
using Hangfire.Redis;
using CS.Manager.Infrastructure.Jobs;

namespace CS.Manager.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            var mySqlConnstr = configuration.GetConnectionString("MasterMysql");
            MasterManagerDB = new FreeSql.FreeSqlBuilder()
               .UseConnectionString(FreeSql.DataType.MySql, mySqlConnstr)
               .UseLogger(loggerFactory.CreateLogger<IFreeSql>())
               .UseAutoSyncStructure(true)
               .UseLazyLoading(true)
               .Build<MasterDb>();
            MasterManagerDB.CodeFirst.SyncStructure(typeof(Users));
        }
        public static IFreeSql<MasterDb> MasterManagerDB { get; private set; }
        // public static IFreeSql<SlaveDb> SlaveManagerDB { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            services.AddSingleton(Configuration);

            // Redis单例模式
            var redisConStr = Configuration.GetConnectionString("DefaultRedis");
            var redisConnect = new CSRedisClient(redisConStr);
            RedisHelper.Initialization(redisConnect);
            services.AddSingleton<IDistributedCache>(new CSRedisCache(RedisHelper.Instance));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            });
            services.AddSingleton<IFreeSql<MasterDb>>(MasterManagerDB);
            services.AddFreeRepository(filter => filter.Apply<ISoftDelete>("SoftDelete", a => a.IsDeleted == false), GetType().Assembly);

            #region Hangfire Job
            var jobConStr = Configuration.GetConnectionString("HangfireRedis");
            var jobConnect = ConnectionMultiplexer.Connect(jobConStr);
            services.Configure<BackgroundJobServerOptions>(options =>
            {
                options.ShutdownTimeout = TimeSpan.FromMinutes(30);
                options.WorkerCount = Math.Min(Environment.ProcessorCount * 5, 20);
                options.Queues = new[] { "default", "apis", "jobs" };
            });
            services.AddHangfire(cfg =>
            {
                //使用Redis存储Job队列
                cfg.UseRedisStorage(jobConnect, new Hangfire.Redis.RedisStorageOptions { Db = 2 });

                // DashboardMetric
                cfg.UseDashboardMetric(DashboardMetrics.ServerCount)
                    .UseDashboardMetric(DashboardMetrics.RecurringJobCount)
                    .UseDashboardMetric(DashboardMetrics.RetriesCount)
                    .UseDashboardMetric(DashboardMetrics.AwaitingCount)
                    .UseDashboardMetric(DashboardMetrics.EnqueuedAndQueueCount)
                    .UseDashboardMetric(DashboardMetrics.ScheduledCount)
                    .UseDashboardMetric(DashboardMetrics.ProcessingCount)
                    .UseDashboardMetric(DashboardMetrics.SucceededCount)
                    .UseDashboardMetric(DashboardMetrics.FailedCount)
                    .UseDashboardMetric(DashboardMetrics.DeletedCount);
                if (JobStorage.Current is Hangfire.Redis.RedisStorage redisStorage)
                {
                    cfg.UseDashboardMetric(
                        redisStorage.GetDashboardMetricFromRedisInfo("使用内存", RedisInfoKeys.used_memory_human));
                    cfg.UseDashboardMetric(
                        redisStorage.GetDashboardMetricFromRedisInfo("高峰内存", RedisInfoKeys.used_memory_peak_human));
                }
                cfg.UseRecurringJob("recurringjobs.json");
                cfg.UseFilter(new AutomaticRetryAttribute { Attempts = 5 });
                cfg.UseNLogLogProvider();
             

            });
            services.AddSingleton<IJobManager, HangfireJobManager>();
            #endregion

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(p =>
                {
                    p.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });


            ////添加认证Cookie信息
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            //    {
            //        options.LoginPath = "/login";
            //        options.LogoutPath = "/logout";
            //        options.AccessDeniedPath = "/login";
            //        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            //        options.SlidingExpiration = true;
            //        options.Cookie.Name = "CS.Manager";
            //    });

            //Token 身份认证
            services.AddAuthentication(ApiTokenOptions.Scheme)
           .AddScheme<ApiTokenOptions, ApiTokenHandler>(ApiTokenOptions.Scheme, p => { });
            // Api文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("CS.Manager.Api", new Info { Title = "后台管理接口文档", Version = "V1" });
                foreach (var file in Directory.EnumerateFiles(AppContext.BaseDirectory, "*.xml"))
                {
                    c.IncludeXmlComments(file, true);
                }

                c.UseReferencedDefinitionsForEnums();
                c.IgnoreObsoleteActions();
                c.SchemaFilter<EnumSchemaFilter>();

            });

            //添加对AutoMapper的支持
            Mapper.Initialize(x => AutoMapperHelper.CreateDtoMappings(x));

            services.Configure<MvcOptions>(mvcOptions =>
            {
                mvcOptions.Filters.Add<ValidateResultFilter>();
                mvcOptions.Filters.Add<ActionLogFilter>();
                //mvcOptions.Filters.Add<ApiAuthorizationFilter>();
            });

            //services.BatchRegisterService(new Assembly[] { Assembly.GetExecutingAssembly()
            //    , Assembly.Load("CS.Manager.Application")
            //    , Assembly.Load("CS.Manager.Application")
            //    , Assembly.Load("CS.Manager.EasyNetQ")
            //    , Assembly.Load("CS.Manager.Job")
            //    , Assembly.Load("CS.Repository")
            //}, typeof(ISingletonDependency));

            #region 应用服务
            services.AddTransient<IAuthAppService, AuthAppService>();
            services.AddTransient<IRabbitMqAppService, RabbitMqAppService>();

            #endregion

            #region RabbitMq

            services.AddSingleton<IServiceRegister, ServiceCollectionAdapter>();
            string rabbitMqConnection1 = Configuration.GetConnectionString("RabbitMQ1");
            services.RegisterEasyNetQ(rabbitMqConnection1);
            services.Replace(ServiceDescriptor.Singleton<IConsumerErrorStrategy, ConsumerErrorStategy>());
            services.AddSingleton<IBaseConsumer, TestConsumer>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //使用NLog作为日志记录工具
            loggerFactory.AddNLog();
            //引入Nlog配置文件
            env.ConfigureNLog("NLog.config");

            var dashboardOptions = new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
            };
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", dashboardOptions);
            // Authentication
            app.UseAuthentication();
            //rabbitmq Subscribe
            app.UseRabbitMQSubscribe();
            // HttpLog
            app.UseMiddleware<HttpLogMiddleware>();

            app.UseCors();

            app.UseMvc();

            // Api文档
            app.UseSwagger(c => { c.RouteTemplate = "{documentName}/swagger.json"; });
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/CS.Manager.Api/swagger.json", "AdminApi"); });
        }
    }
}
