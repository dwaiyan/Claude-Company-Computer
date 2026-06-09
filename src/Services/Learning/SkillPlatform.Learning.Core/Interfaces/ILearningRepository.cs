using SkillPlatform.Learning.Core.Entities;

namespace SkillPlatform.Learning.Core.Interfaces;

public interface ILearningRepository
{
    // Learning Paths
    Task<LearningPath?> GetPathByUserAsync(Guid userId);
    Task<LearningPath> UpsertPathAsync(LearningPath path);

    // Skill Assessments
    Task<List<SkillAssessment>> GetAssessmentsByUserAsync(Guid userId);
    Task<SkillAssessment> UpsertAssessmentAsync(SkillAssessment assessment);

    // Check-ins
    Task<CheckIn> AddCheckInAsync(CheckIn checkIn);
    Task<List<CheckIn>> GetCheckInsByUserAsync(Guid userId, DateTime? from = null);
    Task<int> GetStreakDaysAsync(Guid userId);

    // Assessments (tests)
    Task<List<Assessment>> GetAssessmentsByNodeAsync(Guid nodeId);
    Task<Assessment?> GetAssessmentByIdAsync(Guid id);
    Task<AssessmentRecord> SubmitRecordAsync(AssessmentRecord record);
    Task<List<AssessmentRecord>> GetRecordsByUserAsync(Guid userId);
}
