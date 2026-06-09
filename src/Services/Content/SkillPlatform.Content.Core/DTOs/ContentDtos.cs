using System.Text.Json.Serialization;

namespace SkillPlatform.Content.Core.DTOs;

public class TechTreeDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("children")]
    public List<TechTreeDto> Children { get; set; } = new();
}

public class TechNodeDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("parentId")]
    public Guid? ParentId { get; set; }

    [JsonPropertyName("children")]
    public List<TechNodeDto> Children { get; set; } = new();

    [JsonPropertyName("resourceCount")]
    public int ResourceCount { get; set; }

    [JsonPropertyName("questionCount")]
    public int QuestionCount { get; set; }
}

public class ResourceDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = "article";

    [JsonPropertyName("difficulty")]
    public int Difficulty { get; set; }
}

public class InterviewQuestionDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("question")]
    public string Question { get; set; } = string.Empty;

    [JsonPropertyName("answerTip")]
    public string AnswerTip { get; set; } = string.Empty;

    [JsonPropertyName("difficulty")]
    public int Difficulty { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; } = "general";
}
