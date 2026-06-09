using Microsoft.EntityFrameworkCore;
using SkillPlatform.Collaboration.Core.Entities;
using SkillPlatform.Collaboration.Core.Interfaces;
using SkillPlatform.Collaboration.Infra.Data;

namespace SkillPlatform.Collaboration.Infra.Repositories;

public class CollaborationRepository : ICollaborationRepository
{
    private readonly CollaborationDbContext _context;
    public CollaborationRepository(CollaborationDbContext context) => _context = context;

    public async Task<List<Note>> GetNotesByNodeAsync(Guid nodeId, string? status = null)
    {
        var query = _context.Notes.Where(n => n.NodeId == nodeId);
        if (status != null) query = query.Where(n => n.Status == status);
        return await query.OrderByDescending(n => n.CreatedAt).ToListAsync();
    }

    public async Task<Note?> GetNoteByIdAsync(Guid id)
        => await _context.Notes.FindAsync(id);

    public async Task<Note> CreateNoteAsync(Note note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
        return note;
    }

    public async Task<Note> UpdateNoteAsync(Note note)
    {
        _context.Notes.Update(note);
        await _context.SaveChangesAsync();
        return note;
    }

    public async Task<List<Discussion>> GetDiscussionsByNodeAsync(Guid nodeId)
    {
        return await _context.Discussions
            .Where(d => d.NodeId == nodeId && d.ParentId == null)
            .Include(d => d.Replies)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
    }

    public async Task<Discussion> AddDiscussionAsync(Discussion discussion)
    {
        _context.Discussions.Add(discussion);
        await _context.SaveChangesAsync();
        return discussion;
    }

    public async Task<Contribution> SubmitContributionAsync(Contribution contribution)
    {
        _context.Contributions.Add(contribution);
        await _context.SaveChangesAsync();
        return contribution;
    }

    public async Task<List<Contribution>> GetPendingContributionsAsync()
    {
        return await _context.Contributions
            .Where(c => c.Status == "pending")
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Contribution> ReviewContributionAsync(Guid id, string status, Guid reviewerId, string? comment)
    {
        var c = await _context.Contributions.FindAsync(id)
            ?? throw new InvalidOperationException("贡献不存在");
        c.Status = status;
        c.ReviewedBy = reviewerId;
        c.ReviewComment = comment;
        await _context.SaveChangesAsync();
        return c;
    }
}
