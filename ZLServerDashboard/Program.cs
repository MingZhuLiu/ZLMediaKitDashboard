using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ZLServerDashboard
{
    public class Program
    {
    private static IHost webHost = null;
        public static IHost WebHost { get => webHost; }
        public static void Main(string[] args)
        {


            webHost = CreateHostBuilder(args).Build();
            webHost.Run();
        }

        public static ILogger<T> GetLogger<T>()
        {
            var t = webHost.Services.GetRequiredService<ILogger<T>>();
            return t;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            
           

            return  Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    
                    
                    webBuilder.UseStartup<Startup>();
                });
        }
           
    }
}
