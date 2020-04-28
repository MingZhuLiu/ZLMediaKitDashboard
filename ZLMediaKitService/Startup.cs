using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZLMediaKitService.DataBase;
using ZLMediaKitService.Model.Config;

namespace ZLMediaKitService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }




        //  public async static Task InitDB(IServiceProvider service)
        // {
        //     var db = service.GetService<ZLMediaKitContext>();
        // }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddZLMediaKitDB(Configuration);
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // app.useZL
            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // InitIdentityServerDataBase(app);
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="app"></param>
        public void InitIdentityServerDataBase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ZLMediaKitContext>();
                db.Database.Migrate();
                db.Database.EnsureCreated();
                if (db.Database != null && db.Database.EnsureCreated())
                {


                    UserEntity article = new UserEntity
                    {
                        Account = "test",
                        Password = "1xxx"
                    };

                    db.Users.Add(article);
                    db.SaveChanges();
                }
                var a = db.Users.ToList();
            }
        }
    }
}
