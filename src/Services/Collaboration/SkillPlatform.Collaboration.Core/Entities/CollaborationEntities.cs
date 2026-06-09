namespace SkillPlatform.Collaboration.Core.Entities;

public class Note
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid NodeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty; // Markdown
    public string Status { get; set; } = "published"; // draft, published
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class Discussion
{
    public Guid Id { get; set; }
    public Guid NodeId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public Discussion? Parent { get; set; }
    public ICollection<Discussion> Replies { get; set; } = new List<Discussion>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Contribution
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; } = "resource"; // resource, note, question
    public string Target { get; set; } = string.Empty;
    public string Status { get; set; } = "pending"; // pending, approved, rejected
    public Guid? ReviewedBy { get; set; }
    public string? ReviewComment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
