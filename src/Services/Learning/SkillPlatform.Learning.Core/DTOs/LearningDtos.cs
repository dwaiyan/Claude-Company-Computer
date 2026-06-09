using System.Text.Json.Serialization;

namespace SkillPlatform.Learning.Core.DTOs;

public class LearningProfileDto
{
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }

    [JsonPropertyName("streakDays")]
    public int StreakDays { get; set; }

    [JsonPropertyName("totalCheckIns")]
    public int TotalCheckIns { get; set; }

    [JsonPropertyName("weeklyMinutes")]
    public int WeeklyMinutes { get; set; }

    [JsonPropertyName("skillRadar")]
    public List<SkillRadarItem> SkillRadar { get; set; } = new();
}

public class SkillRadarItem
{
    [JsonPropertyName("nodeId")]
    public Guid NodeId { get; set; }

    [JsonPropertyName("nodeTitle")]
    public string NodeTitle { get; set; } = string.Empty;

    [JsonPropertyName("selfScore")]
    public int SelfScore { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }
}

public class CheckInRequest
{
    [JsonPropertyName("durationMinutes")]
    public int DurationMinutes { get; set; }

    [JsonPropertyName("note")]
    public string? Note { get; set; }
}

public class CheckInDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("durationMinutes")]
    public int DurationMinutes { get; set; }

    [JsonPropertyName("note")]
    public string? Note { get; set; }
}

public class AssessmentDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = "choice";

    [JsonPropertyName("nodeId")]
    public Guid NodeId { get; set; }

    [JsonPropertyName("questions")]
    public object Questions { get; set; } = new();

    [JsonPropertyName("timeLimit")]
    public int TimeLimit { get; set; }

    [JsonPropertyName("passScore")]
    public int PassScore { get; set; }
}

public class SubmitAssessmentRequest
{
    [JsonPropertyName("answers")]
    public object Answers { get; set; } = new();

    [JsonPropertyName("startedAt")]
    public DateTime StartedAt { get; set; }
}

public class AssessmentRecordDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("assessmentId")]
    public Guid AssessmentId { get; set; }

    [JsonPropertyName("assessmentTitle")]
    public string AssessmentTitle { get; set; } = string.Empty;

    [JsonPropertyName("score")]
    public int Score { get; set; }

    [JsonPropertyName("passed")]
    public bool Passed { get; set; }

    [JsonPropertyName("finishedAt")]
    public DateTime? FinishedAt { get; set; }
}

public class SkillAssessmentRequest
{
    [JsonPropertyName("nodeId")]
    public Guid NodeId { get; set; }

    [JsonPropertyName("selfScore")]
    public int SelfScore { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }
}
