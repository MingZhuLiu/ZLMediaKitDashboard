using Microsoft.EntityFrameworkCore;

namespace ZLMediaKitService.DataBase
{
    public class ZLMediaKitContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public ZLMediaKitContext(DbContextOptions<ZLMediaKitContext> options) : base(options)
        {
            // Database.Migrate();
            // Database.EnsureCreated();

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.InitModel();
            // modelBuilder.InitBasModel();
            // modelBuilder.InitReportModel();
            // modelBuilder.InitProcModel();
#if DEBUG
            //modelBuilder.InitMetaData();
            //modelBuilder.InitEMERTestData();

            //modelBuilder.InitSSTestData();
#endif
        }
    }

}