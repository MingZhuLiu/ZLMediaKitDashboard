using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ZLServerDashboard.Commons;
using ZLServerDashboard.DataBase;
using ZLServerDashboard.Hubs;
using ZLServerDashboard.Implements;
using ZLServerDashboard.Interface;

namespace ZLServerDashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _instance = this;
        }

        public IApplicationBuilder App { get; set; }
        private static Startup _instance;
        public static Startup Instance { get => _instance; }

        public IConfiguration Configuration { get; }

        public static String sqlConn = null;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            sqlConn = Configuration.GetValue<String>("MediaPlatContextConnectionStr");
            var options = new DbContextOptionsBuilder<MediaPlatContext>()
          .UseSqlServer(sqlConn)
           .Options;
            // services.AddDbContext(s => new ZhiHuReptileContext(options));
            services.AddDbContext<MediaPlatContext>(options => options.UseSqlServer(sqlConn));


            // services.AddHangfire(r => r.UseSqlServerStorage(sqlConn));
            var redisConn = Configuration.GetValue<String>("RedisConnectStr");


            services.AddCors(options =>
           {
               options.AddPolicy("MyAllowSpecificOrigins",
                   builder =>
                    builder.WithOrigins("http://localhost:8089", "http://192.168.18.12:8091")
                   /*
                   根据自己情况调整


                   如果同时打开 AllowAnyOrigin()
                                       .AllowAnyMethod()
                                       .AllowAnyHeader()
                                       .AllowCredentials());
                   会抛下面这个异常：
                   System.InvalidOperationException: Endpoint AnXin.DigitalFirePlatform.WebApi.Controllers.StaticPersonController.Get (AnXin.DigitalFirePlatform.WebApi) contains CORS metadata, but a middleware was not found that supports CORS.
                   Configure your application startup by adding app.UseCors() inside the call to Configure(..) in the application startup code. The call to app.UseAuthorization() must appear between app.UseRouting() and app.UseEndpoints(...).
                      at Microsoft.AspNetCore.Routing.EndpointMiddleware.ThrowMissingCorsMiddlewareException(Endpoint endpoint)
                      at Microsoft.AspNetCore.Routing.EndpointMiddleware.Invoke(HttpContext httpContext)
                      at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
                      at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware.Invoke(HttpContext context)



                   */
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials());

           });

            services.AddControllers();

            services.AddControllersWithViews();
            services.AddSignalR();

            services.AddMvc()
                .AddNewtonsoftJson(
                options =>
                {
                    // ����ѭ������
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    // ��ʹ���շ�
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    // ����ʱ���ʽ
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    // ���ֶ�Ϊnullֵ�����ֶβ��᷵�ص�ǰ��
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                }
                ).AddRazorRuntimeCompilation()
                    .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddAutoMapper(typeof(MappingProfile));
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // app.UseHttpsRedirection();
            app.UseCors("MyAllowSpecificOrigins");

            app.UseDefaultFiles();

            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".properties"] = "application/octet-stream";
            provider.Mappings[".bcmap"] = "application/octet-stream";
            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = provider
            });
            app.UseRouting();

            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<WebHub>("/webhub");
            });
            this.App = app;
        }
    }
}
