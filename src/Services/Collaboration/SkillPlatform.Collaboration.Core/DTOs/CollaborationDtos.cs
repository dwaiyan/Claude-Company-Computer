using System.Text.Json.Serialization;

namespace SkillPlatform.Collaboration.Core.DTOs;

public class NoteDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;
    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;
    [JsonPropertyName("nodeId")] public Guid NodeId { get; set; }
    [JsonPropertyName("userId")] public Guid UserId { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; } = "published";
    [JsonPropertyName("viewCount")] public int ViewCount { get; set; }
    [JsonPropertyName("likeCount")] public int LikeCount { get; set; }
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
}

public class CreateNoteRequest
{
    [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;
    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;
    [JsonPropertyName("nodeId")] public Guid NodeId { get; set; }
}

public class DiscussionDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("nodeId")] public Guid NodeId { get; set; }
    [JsonPropertyName("userId")] public Guid UserId { get; set; }
    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;
    [JsonPropertyName("parentId")] public Guid? ParentId { get; set; }
    [JsonPropertyName("replies")] public List<DiscussionDto> Replies { get; set; } = new();
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
}

public class CreateDiscussionRequest
{
    [JsonPropertyName("content")] public string Content { get; set; } = string.Empty;
    [JsonPropertyName("nodeId")] public Guid NodeId { get; set; }
    [JsonPropertyName("parentId")] public Guid? ParentId { get; set; }
}

public class ContributionDto
{
    [JsonPropertyName("id")] public Guid Id { get; set; }
    [JsonPropertyName("userId")] public Guid UserId { get; set; }
    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
    [JsonPropertyName("target")] public string Target { get; set; } = string.Empty;
    [JsonPropertyName("status")] public string Status { get; set; } = "pending";
    [JsonPropertyName("createdAt")] public DateTime CreatedAt { get; set; }
}

public class ReviewRequest
{
    [JsonPropertyName("status")] public string Status { get; set; } = "approved";
    [JsonPropertyName("comment")] public string? Comment { get; set; }
}
