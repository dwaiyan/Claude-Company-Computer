using System.Text.Json;

namespace SkillPlatform.Learning.Core.Entities;

public class LearningPath
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string NodeIdsJson { get; set; } = "[]";
    public string ProgressJson { get; set; } = "{}";
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public int TargetDays { get; set; } = 90;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class SkillAssessment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid NodeId { get; set; }
    public int Level { get; set; } = 1;
    public int SelfScore { get; set; } = 1;
    public DateTime EvaluatedAt { get; set; } = DateTime.UtcNow;
}

public class CheckIn
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public int DurationMinutes { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Assessment
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = "choice"; // choice, code, design
    public Guid NodeId { get; set; }
    public string QuestionsJson { get; set; } = "[]";
    public int TimeLimit { get; set; }
    public int PassScore { get; set; } = 60;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<AssessmentRecord> Records { get; set; } = new List<AssessmentRecord>();
}

public class AssessmentRecord
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AssessmentId { get; set; }
    public Assessment Assessment { get; set; } = null!;
    public string AnswersJson { get; set; } = "[]";
    public int Score { get; set; }
    public bool Passed { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
}
