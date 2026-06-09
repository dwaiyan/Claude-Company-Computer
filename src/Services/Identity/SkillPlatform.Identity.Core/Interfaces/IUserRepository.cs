using SkillPlatform.Identity.Core.Entities;

namespace SkillPlatform.Identity.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task<List<Role>> GetUserRolesAsync(Guid userId);
    Task<List<Permission>> GetUserPermissionsAsync(Guid userId);
    Task<Role?> GetRoleByNameAsync(string name);
}
