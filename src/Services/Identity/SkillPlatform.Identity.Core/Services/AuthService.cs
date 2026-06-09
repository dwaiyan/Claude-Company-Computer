using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkillPlatform.Identity.Core.DTOs;
using SkillPlatform.Identity.Core.Entities;
using SkillPlatform.Identity.Core.Interfaces;

namespace SkillPlatform.Identity.Core.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new InvalidOperationException("邮箱或密码错误");

        if (!VerifyPassword(request.Password, user.PasswordHash))
            throw new InvalidOperationException("邮箱或密码错误");

        var roles = await _userRepository.GetUserRolesAsync(user.Id);
        return GenerateAuthResponse(user, roles);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.EmailExistsAsync(request.Email))
            throw new InvalidOperationException("该邮箱已注册");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password),
            Department = request.Department,
            Title = request.Title,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        var memberRole = await _userRepository.GetRoleByNameAsync("member");
        if (memberRole != null)
        {
            user.UserRoles = new List<UserRole>
            {
                new UserRole { UserId = user.Id, RoleId = memberRole.Id }
            };
            await _userRepository.UpdateAsync(user);
        }

        var roles = new List<Role>();
        if (memberRole != null) roles.Add(memberRole);

        return GenerateAuthResponse(user, roles);
    }

    public Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException("Refresh token 将在 P2 实现");
    }

    private AuthResponse GenerateAuthResponse(User user, List<Role> roles)
    {
        var secretKey = _configuration["Jwt:SecretKey"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
            signingCredentials: credentials
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = GenerateRefreshToken(),
            ExpiresIn = expiresInMinutes * 60,
            User = MapToUserDto(user, roles)
        };
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private static UserDto MapToUserDto(User user, List<Role> roles)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Avatar = user.Avatar,
            Title = user.Title,
            Department = user.Department,
            Roles = roles.Select(r => r.Name).ToList()
        };
    }
}
