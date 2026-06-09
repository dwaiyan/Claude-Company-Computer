using Microsoft.EntityFrameworkCore;
using SkillPlatform.Identity.Core.Entities;

namespace SkillPlatform.Identity.Infra.Data;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("permissions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("user_roles");
            entity.HasKey(e => new { e.UserId, e.RoleId });
            entity.HasOne(e => e.User).WithMany(u => u.UserRoles).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Role).WithMany(r => r.UserRoles).HasForeignKey(e => e.RoleId);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("role_permissions");
            entity.HasKey(e => new { e.RoleId, e.PermissionId });
            entity.HasOne(e => e.Role).WithMany(r => r.RolePermissions).HasForeignKey(e => e.RoleId);
            entity.HasOne(e => e.Permission).WithMany(p => p.RolePermissions).HasForeignKey(e => e.PermissionId);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var adminRoleId = Guid.Parse("a0000000-0000-0000-0000-000000000001");
        var memberRoleId = Guid.Parse("a0000000-0000-0000-0000-000000000002");

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = adminRoleId, Name = "admin", Description = "管理员" },
            new Role { Id = memberRoleId, Name = "member", Description = "普通成员" }
        );

        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = Guid.Parse("b0000000-0000-0000-0000-000000000001"), Code = "content:write", Description = "编辑内容" },
            new Permission { Id = Guid.Parse("b0000000-0000-0000-0000-000000000002"), Code = "content:review", Description = "审核内容" },
            new Permission { Id = Guid.Parse("b0000000-0000-0000-0000-000000000003"), Code = "users:manage", Description = "管理用户" },
            new Permission { Id = Guid.Parse("b0000000-0000-0000-0000-000000000004"), Code = "content:view", Description = "查看内容" }
        );

        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = adminRoleId, PermissionId = Guid.Parse("b0000000-0000-0000-0000-000000000001") },
            new RolePermission { RoleId = adminRoleId, PermissionId = Guid.Parse("b0000000-0000-0000-0000-000000000002") },
            new RolePermission { RoleId = adminRoleId, PermissionId = Guid.Parse("b0000000-0000-0000-0000-000000000003") },
            new RolePermission { RoleId = adminRoleId, PermissionId = Guid.Parse("b0000000-0000-0000-0000-000000000004") },
            new RolePermission { RoleId = memberRoleId, PermissionId = Guid.Parse("b0000000-0000-0000-0000-000000000004") }
        );
    }
}
