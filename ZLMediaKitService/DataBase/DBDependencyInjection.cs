using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZLMediaKitService.Model.Config;

namespace ZLMediaKitService.DataBase
{

    static partial class DBDependencyInjection
    {
        public static void AddZLMediaKitDB(this IServiceCollection services, IConfiguration configuration)
        {
            // var dbconfig=configuration["ZLMediaKitDataBase:DataBaseType"]?? throw new ArgumentNullException();
            var dbtype = configuration["ZLMediaKitDataBase:DataBaseType"] ?? throw new ArgumentNullException();
            dbtype = dbtype.ToLower();
            var connectionstr = configuration["ZLMediaKitDataBase:ConnectStr"] ?? throw new ArgumentNullException();
            // var options = new DbContextOptionsBuilder<ZLMediaKitContext>();


            services.AddDbContext<ZLMediaKitContext>(options =>
            {

#if DEBUG
                options.EnableSensitiveDataLogging(true);
#endif
                switch (dbtype)
                {
                    case "mysql":
                        options.UseMySql(connectionstr);
                        break;
                    case "sqlserver":
                        options.UseSqlServer(connectionstr);
                        break;
                    case "sqlite":
                        options.UseSqlite(connectionstr);
                        break;
                    case "postgresql":
                        options.UseNpgsql(connectionstr);
                        break;
                    default:
                        throw new NotImplementedException();
                }

            });


        }
        public static void InitModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
           {
               entity.HasIndex(e => e.Account).IsUnique();
               entity.HasIndex(e => e.SortId);
               // entity.HasMany(e => e.Rels).WithOne(rel => rel.User);
           });
        }
    }

}