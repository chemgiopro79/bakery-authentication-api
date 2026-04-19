using Microsoft.EntityFrameworkCore;
using One.MDM.Authentication.Model;
using System.Data;
using System.Numerics;

namespace One.MDM.Authentication.Services
{
    public class BakeryDbContext : DbContext
    {
        public BakeryDbContext(DbContextOptions<BakeryDbContext> options) : base(options) { }

        // bảng Users
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Entities> Entities { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ánh xạ entity User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user", "public"); // ánh xạ tới public."user"
                entity.HasKey(u => u.Id);         // khóa chính

                entity.Property(u => u.Id)
                      .HasColumnName("id")
                      .HasDefaultValueSql("gen_random_uuid()"); // tự sinh GUID

                entity.Property(p => p.Status).HasColumnName("status");
                entity.Property(u => u.Code)
                     .HasColumnName("code")
                     .HasMaxLength(250)
                     .IsRequired();
                entity.Property(u => u.Username)
                      .HasColumnName("user_name")
                      .HasMaxLength(250)
                      .IsRequired();

                entity.Property(u => u.Password)
                      .HasColumnName("password")
                      .HasMaxLength(250)
                      .IsRequired();

                entity.Property(u => u.CreatedDate)
                      .HasColumnName("created_date")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(u => u.CreatedBy)
                     .HasColumnName("created_by")
                     .HasMaxLength(250);

                entity.Property(u => u.ModifiedDate)
                      .HasColumnName("modified_date")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(u => u.ModifiedBy)
                             .HasColumnName("modified_by")
                             .HasMaxLength(250);
            });
            // Ánh xạ entity Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role", "public");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Id)
                      .HasColumnName("id")
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(r => r.Name)
                      .HasColumnName("name")
                      .HasMaxLength(250)
                      .IsRequired();

                entity.Property(u => u.CreatedDate)
                        .HasColumnName("created_date")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(u => u.CreatedBy)
                     .HasColumnName("created_by")
                     .HasMaxLength(250);

                entity.Property(u => u.ModifiedDate)
                      .HasColumnName("modified_date")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(u => u.ModifiedBy)
                             .HasColumnName("modified_by")
                             .HasMaxLength(250);


            });
            // Ánh xạ entity UserRole
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_role", "public");

                // Composite key
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.Property(ur => ur.UserId)
                      .HasColumnName("user_id")
                      .IsRequired();

                entity.Property(ur => ur.RoleId)
                      .HasColumnName("role_id")
                      .IsRequired();

                // Quan hệ với User
                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId)
                      .HasConstraintName("fk_role_user");

                // Quan hệ với Role
                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId)
                      .HasConstraintName("fk_privilege_role");
            });
            // Ánh xạ entity Role
            modelBuilder.Entity<Entities>(entity =>
            {
                entity.ToTable("entities", "public");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Id)
                     .HasColumnName("id")
                     .ValueGeneratedOnAdd();

                entity.Property(r => r.Name)
                      .HasColumnName("name")
                      .HasMaxLength(250)
                      .IsRequired();

                entity.Property(u => u.CreatedDate)
                        .HasColumnName("created_date")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(u => u.CreatedBy)
                     .HasColumnName("created_by")
                     .HasMaxLength(250);

                entity.Property(u => u.ModifiedDate)
                      .HasColumnName("modified_date")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(u => u.ModifiedBy)
                             .HasColumnName("modified_by")
                             .HasMaxLength(250);


            });
            // Ánh xạ entity Privilege
            modelBuilder.Entity<Privilege>(entity =>
            {
                entity.ToTable("privilege", "public");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                      .HasColumnName("id")
                      .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(p => p.TableId)
                     .HasColumnName("table_id")
                     .IsRequired();

                entity.Property(p => p.TableName)
                      .HasColumnName("table_name")
                      .HasMaxLength(250)
                      .IsRequired();

                entity.Property(p => p.View).HasColumnName("view");
                entity.Property(p => p.Read).HasColumnName("read");
                entity.Property(p => p.Write).HasColumnName("write");
                entity.Property(p => p.Update).HasColumnName("update");
                entity.Property(p => p.Delete).HasColumnName("delete");
                entity.Property(p => p.Approve).HasColumnName("approve");
                entity.Property(p => p.Assign).HasColumnName("assign");
                entity.Property(p => p.Share).HasColumnName("share");

                entity.Property(p => p.CreatedDate)
                    .HasColumnName("created_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(p => p.CreatedBy)
                     .HasColumnName("created_by")
                     .HasMaxLength(250);

                entity.Property(p => p.ModifiedDate)
                      .HasColumnName("modified_date")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(p => p.ModifiedBy)
                             .HasColumnName("modified_by")
                             .HasMaxLength(250);
                // ánh xạ cột FK user_id
                entity.Property(p => p.RoleId)
                      .HasColumnName("role_id");
                // ánh xạ cột FK user_id
                entity.Property(p => p.TableId)
                      .HasColumnName("table_id");

                entity.HasOne(p => p.Entities)
                      .WithMany()
                      .HasForeignKey(p => p.TableId)
                      .HasConstraintName("fk_privilege_entities");
                entity.HasOne(p => p.Role)
                    .WithMany(r => r.Privileges)   // nếu bạn có ICollection<Privilege> trong Role
                    .HasForeignKey(p => p.RoleId)
                    .HasConstraintName("fk_privilege_role");
            });
        }
    }

    public enum PrivilegeNum
    {
        None = 0,
        Full = 1
    }
}
