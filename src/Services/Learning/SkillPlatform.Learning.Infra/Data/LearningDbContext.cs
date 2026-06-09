using Microsoft.EntityFrameworkCore;
using SkillPlatform.Learning.Core.Entities;

namespace SkillPlatform.Learning.Infra.Data;

public class LearningDbContext : DbContext
{
    public LearningDbContext(DbContextOptions<LearningDbContext> options) : base(options)
    {
    }

    public DbSet<LearningPath> LearningPaths => Set<LearningPath>();
    public DbSet<SkillAssessment> SkillAssessments => Set<SkillAssessment>();
    public DbSet<CheckIn> CheckIns => Set<CheckIn>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<AssessmentRecord> AssessmentRecords => Set<AssessmentRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LearningPath>(entity =>
        {
            entity.ToTable("learning_paths");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId).IsUnique();
        });

        modelBuilder.Entity<SkillAssessment>(entity =>
        {
            entity.ToTable("skill_assessments");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.NodeId }).IsUnique();
        });

        modelBuilder.Entity<CheckIn>(entity =>
        {
            entity.ToTable("check_ins");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.Date });
        });

        modelBuilder.Entity<Assessment>(entity =>
        {
            entity.ToTable("assessments");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.NodeId);
        });

        modelBuilder.Entity<AssessmentRecord>(entity =>
        {
            entity.ToTable("assessment_records");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Assessment).WithMany(a => a.Records).HasForeignKey(e => e.AssessmentId);
        });
    }
}
