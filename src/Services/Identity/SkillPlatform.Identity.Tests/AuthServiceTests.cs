using Microsoft.Extensions.Configuration;
using Moq;
using SkillPlatform.Identity.Core.DTOs;
using SkillPlatform.Identity.Core.Entities;
using SkillPlatform.Identity.Core.Interfaces;
using SkillPlatform.Identity.Core.Services;
using Xunit;

namespace SkillPlatform.Identity.Tests;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _repoMock;
    private readonly IConfiguration _config;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _repoMock = new Mock<IUserRepository>();

        var configValues = new Dictionary<string, string>
        {
            { "Jwt:SecretKey", "TestSecretKey!MustBeAtLeast32Characters!" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" },
            { "Jwt:ExpiresInMinutes", "60" }
        };
        _config = new ConfigurationBuilder().AddInMemoryCollection(configValues!).Build();
        _sut = new AuthService(_repoMock.Object, _config);
    }

    [Fact]
    public async Task Register_WithNewEmail_ReturnsAuthResponse()
    {
        _repoMock.Setup(r => r.EmailExistsAsync("new@test.com")).ReturnsAsync(false);
        _repoMock.Setup(r => r.GetRoleByNameAsync("member")).ReturnsAsync(new Role
        {
            Id = Guid.NewGuid(),
            Name = "member",
            Description = "成员"
        });
        _repoMock.Setup(r => r.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

        var request = new RegisterRequest
        {
            Username = "testuser",
            Email = "new@test.com",
            Password = "Test123!",
            Department = "Engineering"
        };

        var result = await _sut.RegisterAsync(request);

        Assert.NotNull(result);
        Assert.NotEmpty(result.AccessToken);
        Assert.NotEmpty(result.RefreshToken);
        Assert.Equal(3600, result.ExpiresIn);
        Assert.Equal("new@test.com", result.User.Email);
        Assert.Equal("testuser", result.User.Username);
        Assert.Contains("member", result.User.Roles);
    }

    [Fact]
    public async Task Register_WithExistingEmail_ThrowsInvalidOperation()
    {
        _repoMock.Setup(r => r.EmailExistsAsync("exists@test.com")).ReturnsAsync(true);

        var request = new RegisterRequest
        {
            Username = "existing",
            Email = "exists@test.com",
            Password = "Test123!"
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.RegisterAsync(request));
        Assert.Equal("该邮箱已注册", ex.Message);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsAuthResponse()
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("Test123!");
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "user@test.com",
            PasswordHash = passwordHash
        };

        _repoMock.Setup(r => r.GetByEmailAsync("user@test.com")).ReturnsAsync(user);
        _repoMock.Setup(r => r.GetUserRolesAsync(user.Id))
            .ReturnsAsync(new List<Role>
            {
                new Role { Id = Guid.NewGuid(), Name = "member", Description = "成员" }
            });

        var request = new LoginRequest
        {
            Email = "user@test.com",
            Password = "Test123!"
        };

        var result = await _sut.LoginAsync(request);

        Assert.NotNull(result);
        Assert.NotEmpty(result.AccessToken);
        Assert.Equal("testuser", result.User.Username);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ThrowsInvalidOperation()
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword");
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@test.com",
            PasswordHash = passwordHash
        };

        _repoMock.Setup(r => r.GetByEmailAsync("user@test.com")).ReturnsAsync(user);

        var request = new LoginRequest
        {
            Email = "user@test.com",
            Password = "WrongPassword"
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.LoginAsync(request));
        Assert.Equal("邮箱或密码错误", ex.Message);
    }

    [Fact]
    public async Task Login_WithNonExistentEmail_ThrowsInvalidOperation()
    {
        _repoMock.Setup(r => r.GetByEmailAsync("nobody@test.com")).ReturnsAsync((User?)null);

        var request = new LoginRequest
        {
            Email = "nobody@test.com",
            Password = "Anything"
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.LoginAsync(request));
        Assert.Equal("邮箱或密码错误", ex.Message);
    }
}
