using SkillPlatform.Collaboration.Core.Entities;

namespace SkillPlatform.Collaboration.Core.Interfaces;

public interface ICollaborationRepository
{
    // Notes
    Task<List<Note>> GetNotesByNodeAsync(Guid nodeId, string? status = null);
    Task<Note?> GetNoteByIdAsync(Guid id);
    Task<Note> CreateNoteAsync(Note note);
    Task<Note> UpdateNoteAsync(Note note);

    // Discussions
    Task<List<Discussion>> GetDiscussionsByNodeAsync(Guid nodeId);
    Task<Discussion> AddDiscussionAsync(Discussion discussion);

    // Contributions
    Task<Contribution> SubmitContributionAsync(Contribution contribution);
    Task<List<Contribution>> GetPendingContributionsAsync();
    Task<Contribution> ReviewContributionAsync(Guid id, string status, Guid reviewerId, string? comment);
}
