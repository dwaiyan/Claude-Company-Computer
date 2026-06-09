using Microsoft.EntityFrameworkCore;
using SkillPlatform.Content.Core.Entities;

namespace SkillPlatform.Content.Infra.Data;

public class ContentDbContext : DbContext
{
    public ContentDbContext(DbContextOptions<ContentDbContext> options) : base(options)
    {
    }

    public DbSet<TechTree> TechTrees => Set<TechTree>();
    public DbSet<TechNode> TechNodes => Set<TechNode>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<InterviewQuestion> InterviewQuestions => Set<InterviewQuestion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TechTree>(entity =>
        {
            entity.ToTable("tech_trees");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Parent).WithMany(e => e.Children).HasForeignKey(e => e.ParentId);
        });

        modelBuilder.Entity<TechNode>(entity =>
        {
            entity.ToTable("tech_nodes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Tree).WithMany(t => t.Nodes).HasForeignKey(e => e.TreeId);
            entity.HasOne(e => e.Parent).WithMany(e => e.Children).HasForeignKey(e => e.ParentId);
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.ToTable("resources");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Url).IsRequired().HasMaxLength(500);
            entity.HasOne(e => e.Node).WithMany(n => n.Resources).HasForeignKey(e => e.NodeId);
        });

        modelBuilder.Entity<InterviewQuestion>(entity =>
        {
            entity.ToTable("interview_questions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Question).IsRequired().HasMaxLength(500);
            entity.HasOne(e => e.Node).WithMany(n => n.Questions).HasForeignKey(e => e.NodeId);
        });
    }
}
