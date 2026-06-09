using SkillPlatform.Content.Core.Entities;

namespace SkillPlatform.Content.Core.Interfaces;

public interface ITechTreeRepository
{
    Task<List<TechTree>> GetAllAsync();
    Task<TechTree?> GetByIdAsync(Guid id);
    Task<List<TechNode>> GetNodesByTreeAsync(Guid treeId);
    Task<TechNode?> GetNodeByIdAsync(Guid id);
    Task<List<Resource>> GetResourcesByNodeAsync(Guid nodeId);
    Task<Resource> AddResourceAsync(Resource resource);
    Task<List<InterviewQuestion>> GetQuestionsByNodeAsync(Guid nodeId, int? difficulty = null);
}
