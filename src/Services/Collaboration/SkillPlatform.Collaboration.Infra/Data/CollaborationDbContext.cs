using Microsoft.EntityFrameworkCore;
using SkillPlatform.Collaboration.Core.Entities;

namespace SkillPlatform.Collaboration.Infra.Data;

public class CollaborationDbContext : DbContext
{
    public CollaborationDbContext(DbContextOptions<CollaborationDbContext> options) : base(options) { }

    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Discussion> Discussions => Set<Discussion>();
    public DbSet<Contribution> Contributions => Set<Contribution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Note>(entity =>
        {
            entity.ToTable("notes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Content).IsRequired();
            entity.HasIndex(e => e.NodeId);
            entity.HasIndex(e => e.UserId);
        });

        modelBuilder.Entity<Discussion>(entity =>
        {
            entity.ToTable("discussions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
            entity.HasOne(e => e.Parent).WithMany(e => e.Replies).HasForeignKey(e => e.ParentId);
            entity.HasIndex(e => e.NodeId);
        });

        modelBuilder.Entity<Contribution>(entity =>
        {
            entity.ToTable("contributions");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.UserId);
        });
    }
}
