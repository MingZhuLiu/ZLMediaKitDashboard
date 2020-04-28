using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLMediaKitService
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    public static class WebHostMigrationExtensions
    {
        /// <summary>
        /// 初始化database方法
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="host"></param>
        /// <param name="sedder"></param>
        /// <returns></returns>
        public static IWebHost MigrateDbContext<TContext>(this IWebHost host, Action<TContext, IServiceProvider> sedder)
            where TContext : ZLMediaKitService.DataBase.ZLMediaKitContext
        {
            //创建数据库实例在本区域有效
            using (var scope=host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                var context = services.GetService<TContext>();
                try
                {
                    context.Database.Migrate();//初始化database
                    sedder(context, services);
                    logger.LogInformation($"执行DbContext{typeof(TContext).Name} seed 成功");
                }
                catch ( Exception ex)
                {
                    logger.LogError(ex, $"执行dbcontext {typeof(TContext).Name}  seed失败");
                }
            }
            return host;
        }
    }
}