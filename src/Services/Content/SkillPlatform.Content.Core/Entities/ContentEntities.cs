namespace SkillPlatform.Content.Core.Entities;

public class TechTree
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int SortOrder { get; set; }
    public Guid? ParentId { get; set; }
    public TechTree? Parent { get; set; }
    public ICollection<TechTree> Children { get; set; } = new List<TechTree>();
    public ICollection<TechNode> Nodes { get; set; } = new List<TechNode>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class TechNode
{
    public Guid Id { get; set; }
    public Guid TreeId { get; set; }
    public TechTree Tree { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Level { get; set; }
    public int SortOrder { get; set; }
    public Guid? ParentId { get; set; }
    public TechNode? Parent { get; set; }
    public ICollection<TechNode> Children { get; set; } = new List<TechNode>();
    public ICollection<Resource> Resources { get; set; } = new List<Resource>();
    public ICollection<InterviewQuestion> Questions { get; set; } = new List<InterviewQuestion>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Resource
{
    public Guid Id { get; set; }
    public Guid NodeId { get; set; }
    public TechNode Node { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Type { get; set; } = "article"; // article, video, code, github
    public int Difficulty { get; set; } = 1;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class InterviewQuestion
{
    public Guid Id { get; set; }
    public Guid NodeId { get; set; }
    public TechNode Node { get; set; } = null!;
    public string Question { get; set; } = string.Empty;
    public string AnswerTip { get; set; } = string.Empty;
    public int Difficulty { get; set; } = 1;
    public string Category { get; set; } = "general";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
