using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ZLServerDashboard.DataBase
{
    public partial class MediaPlatContext : DbContext
    {
        public MediaPlatContext()
        {
        }

        public MediaPlatContext(DbContextOptions<MediaPlatContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbApplication> TbApplication { get; set; }
        public virtual DbSet<TbDomain> TbDomain { get; set; }
        public virtual DbSet<TbMenu> TbMenu { get; set; }
        public virtual DbSet<TbMenuRole> TbMenuRole { get; set; }
        public virtual DbSet<TbRole> TbRole { get; set; }
        public virtual DbSet<TbStreamProxy> TbStreamProxy { get; set; }
        public virtual DbSet<TbUser> TbUser { get; set; }
        public virtual DbSet<TbUserRole> TbUserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Startup.sqlConn);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbApplication>(entity =>
            {
                entity.ToTable("Tb_Application");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.App)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreateTs).HasColumnType("datetime");

                entity.Property(e => e.Remark)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTs).HasColumnType("datetime");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TbApplicationCreateByNavigation)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_APPLI_REFERENCE_TB_USER_CREA");

                entity.HasOne(d => d.UpdateByNavigation)
                    .WithMany(p => p.TbApplicationUpdateByNavigation)
                    .HasForeignKey(d => d.UpdateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_APPLI_REFERENCE_TB_USER_UPDA");
            });

            modelBuilder.Entity<TbDomain>(entity =>
            {
                entity.ToTable("Tb_Domain");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTs).HasColumnType("datetime");

                entity.Property(e => e.DomainName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Remark)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTs).HasColumnType("datetime");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TbDomainCreateByNavigation)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_DOMAI_REFERENCE_TB_USER_CREA");

                entity.HasOne(d => d.UpdateByNavigation)
                    .WithMany(p => p.TbDomainUpdateByNavigation)
                    .HasForeignKey(d => d.UpdateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_DOMAI_REFERENCE_TB_USER_UPDA");
            });

            modelBuilder.Entity<TbMenu>(entity =>
            {
                entity.ToTable("Tb_Menu");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTs)
                    .HasColumnName("CreateTS")
                    .HasColumnType("datetime");

                entity.Property(e => e.Icon)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId).HasColumnName("ParentID");

                entity.Property(e => e.Url)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK__Tb_Menu__ParentI__3E52440B");
            });

            modelBuilder.Entity<TbMenuRole>(entity =>
            {
                entity.HasKey(e => new { e.MenuId, e.RoleId })
                    .HasName("PK__Tb_MenuR__71317EB34310A3EE");

                entity.ToTable("Tb_MenuRole");

                entity.Property(e => e.MenuId).HasColumnName("MenuID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.TbMenuRole)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tb_MenuRo__MenuI__3F466844");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TbMenuRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tb_MenuRo__RoleI__403A8C7D");
            });

            modelBuilder.Entity<TbRole>(entity =>
            {
                entity.ToTable("Tb_Role");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CreateTs)
                    .HasColumnName("CreateTS")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbStreamProxy>(entity =>
            {
                entity.ToTable("Tb_StreamProxy");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateTs).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Remark)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.SourceUrl)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.StreamName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTs).HasColumnType("datetime");

                entity.HasOne(d => d.App)
                    .WithMany(p => p.TbStreamProxy)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_STREA_REFERENCE_TB_APPLI");

                entity.HasOne(d => d.CreateByNavigation)
                    .WithMany(p => p.TbStreamProxyCreateByNavigation)
                    .HasForeignKey(d => d.CreateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_STREA_REFERENCE_TB_USER_CREA");

                entity.HasOne(d => d.Domain)
                    .WithMany(p => p.TbStreamProxy)
                    .HasForeignKey(d => d.DomainId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_STREA_REFERENCE_TB_DOMAI");

                entity.HasOne(d => d.UpdateByNavigation)
                    .WithMany(p => p.TbStreamProxyUpdateByNavigation)
                    .HasForeignKey(d => d.UpdateBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TB_STREA_REFERENCE_TB_USER_UPDA");
            });

            modelBuilder.Entity<TbUser>(entity =>
            {
                entity.ToTable("Tb_User");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(1000);

                entity.Property(e => e.CreateTs).HasColumnType("datetime");

                entity.Property(e => e.LoginName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LoginPasswd)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Tel)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK__Tb_UserR__AF27604F12FC9509");

                entity.ToTable("Tb_UserRole");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TbUserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tb_UserRo__RoleI__4222D4EF");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TbUserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tb_UserRo__UserI__412EB0B6");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
