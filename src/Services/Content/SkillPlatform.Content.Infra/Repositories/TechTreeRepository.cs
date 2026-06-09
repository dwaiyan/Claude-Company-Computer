using Microsoft.EntityFrameworkCore;
using SkillPlatform.Content.Core.Entities;
using SkillPlatform.Content.Core.Interfaces;
using SkillPlatform.Content.Infra.Data;

namespace SkillPlatform.Content.Infra.Repositories;

public class TechTreeRepository : ITechTreeRepository
{
    private readonly ContentDbContext _context;

    public TechTreeRepository(ContentDbContext context)
    {
        _context = context;
    }

    public async Task<List<TechTree>> GetAllAsync()
    {
        return await _context.TechTrees
            .Include(t => t.Children)
            .OrderBy(t => t.SortOrder)
            .ToListAsync();
    }

    public async Task<TechTree?> GetByIdAsync(Guid id)
    {
        return await _context.TechTrees
            .Include(t => t.Children)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<TechNode>> GetNodesByTreeAsync(Guid treeId)
    {
        return await _context.TechNodes
            .Where(n => n.TreeId == treeId)
            .Include(n => n.Children)
            .Include(n => n.Resources)
            .Include(n => n.Questions)
            .OrderBy(n => n.SortOrder)
            .ToListAsync();
    }

    public async Task<TechNode?> GetNodeByIdAsync(Guid id)
    {
        return await _context.TechNodes
            .Include(n => n.Children)
            .Include(n => n.Resources)
            .Include(n => n.Questions)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<List<Resource>> GetResourcesByNodeAsync(Guid nodeId)
    {
        return await _context.Resources
            .Where(r => r.NodeId == nodeId)
            .OrderBy(r => r.Difficulty)
            .ToListAsync();
    }

    public async Task<Resource> AddResourceAsync(Resource resource)
    {
        _context.Resources.Add(resource);
        await _context.SaveChangesAsync();
        return resource;
    }

    public async Task<List<InterviewQuestion>> GetQuestionsByNodeAsync(Guid nodeId, int? difficulty = null)
    {
        var query = _context.InterviewQuestions.Where(q => q.NodeId == nodeId);
        if (difficulty.HasValue)
            query = query.Where(q => q.Difficulty == difficulty.Value);
        return await query.OrderBy(q => q.Difficulty).ToListAsync();
    }
}
