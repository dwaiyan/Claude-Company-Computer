using SkillPlatform.Identity.Core.DTOs;

namespace SkillPlatform.Identity.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
}
