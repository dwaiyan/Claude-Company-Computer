# P1 · 骨架 — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 搭建项目脚手架，实现 Identity 服务 (用户/角色/权限 + JWT 认证)，YARP 网关，Vue 3 前端框架，完成注册登录全流程。

**Architecture:** 4 个项目层 — Common 共享包、Identity 服务 (Core/Infra/Api/Tests)、Gateway 网关、Vue 3 前端。通过 Docker Compose 管理 PostgreSQL 依赖。测试用 xUnit + Moq 覆盖业务逻辑。

**Tech Stack:** .NET 6, ASP.NET Core Web API, EF Core 6, PostgreSQL, YARP, Vue 3 + Vite + Pinia + Axios, Docker Compose, xUnit + Moq

---

## Task 1: 项目骨架与 Docker Compose

**Files:**
- Create: `src/Gateway/SkillPlatform.Gateway/SkillPlatform.Gateway.csproj`
- Create: `src/Gateway/SkillPlatform.Gateway/Program.cs`
- Create: `src/Gateway/SkillPlatform.Gateway/appsettings.json`
- Create: `src/Services/Identity/SkillPlatform.Identity.Api/SkillPlatform.Identity.Api.csproj`
- Create: `src/Services/Identity/SkillPlatform.Identity.Api/Program.cs`
- Create: `src/Services/Identity/SkillPlatform.Identity.Api/appsettings.json`
- Create: `src/Services/Identity/SkillPlatform.Identity.Core/SkillPlatform.Identity.Core.csproj`
- Create: `src/Services/Identity/SkillPlatform.Identity.Infra/SkillPlatform.Identity.Infra.csproj`
- Create: `src/Services/Identity/SkillPlatform.Identity.Tests/SkillPlatform.Identity.Tests.csproj`
- Create: `src/Shared/SkillPlatform.Common/SkillPlatform.Common.csproj`
- Create: `SkillPlatform.sln`
- Create: `docker/docker-compose.yml`
- Create: `docker/docker-compose.dev.yml`

- [ ] **Step 1: Create directory structure**

```bash
mkdir -p src/Gateway/SkillPlatform.Gateway
mkdir -p src/Services/Identity/SkillPlatform.Identity.Api/Controllers
mkdir -p src/Services/Identity/SkillPlatform.Identity.Core/{Entities,Interfaces,Services,DTOs}
mkdir -p src/Services/Identity/SkillPlatform.Identity.Infra/{Data,Repositories}
mkdir -p src/Services/Identity/SkillPlatform.Identity.Tests
mkdir -p src/Shared/SkillPlatform.Common/{Errors,Middleware,Auth}
mkdir -p docker
mkdir -p client
```

- [ ] **Step 2: Create shared SkillPlatform.Common.csproj**

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Http.Abstractions" Version="2.2.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="6.0.0" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="6.0.0" />
    <PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="6.0.0" />
    <PackageReference Include="Serilog" Version="2.12.0" />
    <PackageReference Include="Serilog.AspNetCore" Version="6.1.0" />
    <PackageReference Include="Serilog.Sinks.Console" Version="4.1.0" />
  </ItemGroup>

</Project>
```

- [ ] **Step 3: Create Identity.Core.csproj**

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

- [ ] **Step 4: Create Identity.Infra.csproj**

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\SkillPlatform.Identity.Core\SkillPlatform.Identity.Core.csproj" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="6.0.0" />
  </ItemGroup>

</Project>
```

- [ ] **Step 5: Create Identity.Api.csproj**

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\SkillPlatform.Identity.Core\SkillPlatform.Identity.Core.csproj" />
    <ProjectReference Include="..\SkillPlatform.Identity.Infra\SkillPlatform.Identity.Infra.csproj" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.4.0" />
  </ItemGroup>

</Project>
```

- [ ] **Step 6: Create Identity.Tests.csproj**

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.3.0" />
    <PackageReference Include="xunit" Version="2.4.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.5" />
    <PackageReference Include="Moq" Version="4.18.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\SkillPlatform.Identity.Core\SkillPlatform.Identity.Core.csproj" />
    <ProjectReference Include="..\SkillPlatform.Identity.Infra\SkillPlatform.Identity.Infra.csproj" />
  </ItemGroup>

</Project>
```

- [ ] **Step 7: Create Gateway.csproj**

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Yarp.ReverseProxy" Version="2.1.0" />
  </ItemGroup>

</Project>
```

- [ ] **Step 8: Create docker-compose.dev.yml (基础依赖)**

```yaml
version: '3.9'

services:
  postgres:
    image: postgres:15-alpine
    environment:
      POSTGRES_USER: skilldev
      POSTGRES_PASSWORD: SkillDev@2026
      POSTGRES_DB: identity
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data

volumes:
  pgdata:
```

- [ ] **Step 9: Create docker-compose.yml (全量)**

```yaml
version: '3.9'

services:
  postgres:
    image: postgres:15-alpine
    environment:
      POSTGRES_USER: skilldev
      POSTGRES_PASSWORD: SkillDev@2026
      POSTGRES_DB: identity
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data

  gateway:
    build:
      context: ../src/Gateway/SkillPlatform.Gateway
      dockerfile: Dockerfile
    ports:
      - "8080:80"
    depends_on:
      - identity-api

  identity-api:
    build:
      context: ../src/Services/Identity/SkillPlatform.Identity.Api
      dockerfile: Dockerfile
    environment:
      ConnectionStrings__DefaultConnection: "Host=postgres;Database=identity;Username=skilldev;Password=SkillDev@2026"
    depends_on:
      - postgres

volumes:
  pgdata:
```

- [ ] **Step 10: Create solution file and add projects**

```bash
cd src
dotnet new sln -n SkillPlatform -o .
dotnet sln add Shared/SkillPlatform.Common/SkillPlatform.Common.csproj
dotnet sln add Gateway/SkillPlatform.Gateway/SkillPlatform.Gateway.csproj
dotnet sln add Services/Identity/SkillPlatform.Identity.Core/SkillPlatform.Identity.Core.csproj
dotnet sln add Services/Identity/SkillPlatform.Identity.Infra/SkillPlatform.Identity.Infra.csproj
dotnet sln add Services/Identity/SkillPlatform.Identity.Api/SkillPlatform.Identity.Api.csproj
dotnet sln add Services/Identity/SkillPlatform.Identity.Tests/SkillPlatform.Identity.Tests.csproj
```

- [ ] **Step 11: Restore all packages**

```bash
dotnet restore
```

Expected: All packages restore successfully, no errors.

- [ ] **Step 12: Verify build**

```bash
dotnet build --no-restore
```

Expected: Build succeeds with 0 errors.

- [ ] **Step 13: Commit**

```bash
git add -A
git commit -m "scaffold: solution structure, project files, docker-compose

- SkillPlatform.sln with 6 projects
- Identity service (Core/Infra/Api/Tests)
- Shared Common package
- YARP Gateway project
- Docker Compose for PostgreSQL and services

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

## Task 2: Shared 公共库 SkillPlatform.Common

**Files:**
- Create: `src/Shared/SkillPlatform.Common/Errors/ErrorCode.cs`
- Create: `src/Shared/SkillPlatform.Common/Errors/ApiErrorResponse.cs`
- Create: `src/Shared/SkillPlatform.Common/Middleware/ExceptionMiddleware.cs`
- Create: `src/Shared/SkillPlatform.Common/Auth/JwtExtensions.cs`

- [ ] **Step 1: Create unified error codes**

File: `src/Shared/SkillPlatform.Common/Errors/ErrorCode.cs`

```csharp
namespace SkillPlatform.Common.Errors;

public static class ErrorCode
{
    public const string InvalidCredentials = "AUTH_INVALID_CREDENTIALS";
    public const string UserNotFound = "AUTH_USER_NOT_FOUND";
    public const string EmailAlreadyExists = "AUTH_EMAIL_EXISTS";
    public const string TokenExpired = "AUTH_TOKEN_EXPIRED";
    public const string InvalidToken = "AUTH_INVALID_TOKEN";
    public const string Unauthorized = "AUTH_UNAUTHORIZED";
    public const string Forbidden = "AUTH_FORBIDDEN";
    public const string ValidationFailed = "VALIDATION_FAILED";
    public const string InternalError = "INTERNAL_ERROR";
    public const string ResourceNotFound = "RESOURCE_NOT_FOUND";
}
```

- [ ] **Step 2: Create API error response model**

File: `src/Shared/SkillPlatform.Common/Errors/ApiErrorResponse.cs`

```csharp
using System.Text.Json.Serialization;

namespace SkillPlatform.Common.Errors;

public class ApiErrorResponse
{
    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;

    [JsonPropertyName("detail")]
    public string? Detail { get; init; }

    [JsonPropertyName("traceId")]
    public string TraceId { get; init; } = string.Empty;
}
```

- [ ] **Step 3: Create global exception middleware**

File: `src/Shared/SkillPlatform.Common/Middleware/ExceptionMiddleware.cs`

```csharp
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SkillPlatform.Common.Errors;

namespace SkillPlatform.Common.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UnauthorizedAccessException)
        {
            await WriteErrorResponse(context, HttpStatusCode.Forbidden, ErrorCode.Forbidden, "无权执行此操作");
        }
        catch (InvalidOperationException ex)
        {
            await WriteErrorResponse(context, HttpStatusCode.BadRequest, ErrorCode.ValidationFailed, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "未处理的异常: {Message}", ex.Message);
            await WriteErrorResponse(context, HttpStatusCode.InternalServerError, ErrorCode.InternalError, "服务器内部错误");
        }
    }

    private static async Task WriteErrorResponse(HttpContext context, HttpStatusCode status, string code, string message)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json; charset=utf-8";

        var response = new ApiErrorResponse
        {
            Code = code,
            Message = message,
            TraceId = context.TraceIdentifier
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
```

- [ ] **Step 4: Create JWT extensions**

File: `src/Shared/SkillPlatform.Common/Auth/JwtExtensions.cs`

```csharp
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace SkillPlatform.Common.Auth;

public static class JwtExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        string secretKey,
        string issuer,
        string audience)
    {
        var key = Encoding.UTF8.GetBytes(secretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var json = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        code = Errors.ErrorCode.Unauthorized,
                        message = "请先登录",
                        traceId = context.HttpContext.TraceIdentifier
                    });
                    await context.Response.WriteAsync(json);
                }
            };
        });

        return services;
    }
}
```

- [ ] **Step 5: Build and verify**

```bash
dotnet build src/Shared/SkillPlatform.Common/SkillPlatform.Common.csproj
```

Expected: Build succeeds.

- [ ] **Step 6: Commit**

```bash
git add src/Shared/SkillPlatform.Common/
git commit -m "feat: add SkillPlatform.Common shared library

- Unified error codes (ErrorCode)
- API error response model
- Global exception middleware
- JWT authentication extensions

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

## Task 3: Identity.Core 领域层 (Entities, Interfaces, DTOs, Services)

**Files:**
- Create: `src/Services/Identity/SkillPlatform.Identity.Core/Entities/User.cs`
- Create: `src/Services/Identity/SkillPlatform.Identity.Core/Entities/Role.cs`
- Create: `src/Services/Identity/SkillPlatform.Identity.Core/Entities/Permission.cs`
- Create: `src/Services/Identity/SkillPlatform.Identity.Core/Interfaces/IUserRepository.cs`
- Create: `src/Services/Identity/SkillPlatform.Identity.Core/Interfaces/IAuthService.cs`
- Create: `src/Services/Identity/SkillPlatform.Identity.Core/DTOs/LoginRequest.cs`
- Create: `src/Services/Identity/SkillPlatform.Identity.Core/DTOs/RegisterRequest.cs`
- Create: `src/Services/Identity/SkillPlatform.Identity.Core/DTOs/AuthResponse.cs`
- Create: `src/Services/Identity/SkillPlatform.Identity.Core/DTOs/UserDto.cs`
- Create: `src/Services/Identity/SkillPlatform.Identity.Core/Services/AuthService.cs`

- [ ] **Step 1: Create User entity**

File: `src/Services/Identity/SkillPlatform.Identity.Core/Entities/User.cs`

```csharp
namespace SkillPlatform.Identity.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Title { get; set; }
    public string? Department { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

public class Permission
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

public class UserRole
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}

public class RolePermission
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;
}
```

- [ ] **Step 2: Create IUserRepository interface**

File: `src/Services/Identity/SkillPlatform.Identity.Core/Interfaces/IUserRepository.cs`

```csharp
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
```

- [ ] **Step 3: Create IAuthService interface**

File: `src/Services/Identity/SkillPlatform.Identity.Core/Interfaces/IAuthService.cs`

```csharp
using SkillPlatform.Identity.Core.DTOs;

namespace SkillPlatform.Identity.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
}
```

- [ ] **Step 4: Create DTOs**

File: `src/Services/Identity/SkillPlatform.Identity.Core/DTOs/LoginRequest.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace SkillPlatform.Identity.Core.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "密码不能为空")]
    public string Password { get; set; } = string.Empty;
}
```

File: `src/Services/Identity/SkillPlatform.Identity.Core/DTOs/RegisterRequest.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace SkillPlatform.Identity.Core.DTOs;

public class RegisterRequest
{
    [Required(ErrorMessage = "用户名不能为空")]
    [StringLength(50, MinimumLength = 2)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "密码不能为空")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "密码至少6位")]
    public string Password { get; set; } = string.Empty;

    public string? Department { get; set; }
    public string? Title { get; set; }
}
```

File: `src/Services/Identity/SkillPlatform.Identity.Core/DTOs/AuthResponse.cs`

```csharp
using System.Text.Json.Serialization;

namespace SkillPlatform.Identity.Core.DTOs;

public class AuthResponse
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;

    [JsonPropertyName("expiresIn")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("user")]
    public UserDto User { get; set; } = null!;
}
```

File: `src/Services/Identity/SkillPlatform.Identity.Core/DTOs/UserDto.cs`

```csharp
using System.Text.Json.Serialization;

namespace SkillPlatform.Identity.Core.DTOs;

public class UserDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("department")]
    public string? Department { get; set; }

    [JsonPropertyName("roles")]
    public List<string> Roles { get; set; } = new();
}
```

- [ ] **Step 5: Create AuthService**

File: `src/Services/Identity/SkillPlatform.Identity.Core/Services/AuthService.cs`

```csharp
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
```

- [ ] **Step 6: Add BCrypt.Net NuGet to Core project**

Run:
```bash
dotnet add src/Services/Identity/SkillPlatform.Identity.Core/SkillPlatform.Identity.Core.csproj package BCrypt.Net-Next
dotnet add src/Services/Identity/SkillPlatform.Identity.Core/SkillPlatform.Identity.Core.csproj package Microsoft.Extensions.Configuration.Abstractions
dotnet add src/Services/Identity/SkillPlatform.Identity.Core/SkillPlatform.Identity.Core.csproj package System.IdentityModel.Tokens.Jwt
dotnet add src/Services/Identity/SkillPlatform.Identity.Core/SkillPlatform.Identity.Core.csproj package Microsoft.IdentityModel.Tokens
dotnet add src/Services/Identity/SkillPlatform.Identity.Core/SkillPlatform.Identity.Core.csproj package System.ComponentModel.Annotations
```

- [ ] **Step 7: Build Core project**

```bash
dotnet build src/Services/Identity/SkillPlatform.Identity.Core/SkillPlatform.Identity.Core.csproj
```

Expected: Build succeeds.

- [ ] **Step 8: Commit**

```bash
git add src/Services/Identity/SkillPlatform.Identity.Core/
git commit -m "feat: add Identity.Core domain layer

- User/Role/Permission entities with UserRole/RolePermission join tables
- IUserRepository and IAuthService interfaces
- LoginRequest, RegisterRequest, AuthResponse, UserDto DTOs
- AuthService with JWT generation, BCrypt password hashing

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

## Task 4: Identity.Infra 基础设施层 (DbContext + Repository)

**Files:**
- Create: `src/Services/Identity/SkillPlatform.Identity.Infra/Data/IdentityDbContext.cs`
- Create: `src/Services/Identity/SkillPlatform.Identity.Infra/Repositories/UserRepository.cs`

- [ ] **Step 1: Create IdentityDbContext**

File: `src/Services/Identity/SkillPlatform.Identity.Infra/Data/IdentityDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using SkillPlatform.Identity.Core.Entities;

namespace SkillPlatform.Identity.Infra.Data;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("permissions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Code).IsUnique();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("user_roles");
            entity.HasKey(e => new { e.UserId, e.RoleId });
            entity.HasOne(e => e.User).WithMany(u => u.UserRoles).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Role).WithMany(r => r.UserRoles).HasForeignKey(e => e.RoleId);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("role_permissions");
            entity.HasKey(e => new { e.RoleId, e.PermissionId });
            entity.HasOne(e => e.Role).WithMany(r => r.RolePermissions).HasForeignKey(e => e.RoleId);
            entity.HasOne(e => e.Permission).WithMany(p => p.RolePermissions).HasForeignKey(e => e.PermissionId);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var adminRoleId = Guid.Parse("a0000000-0000-0000-0000-000000000001");
        var memberRoleId = Guid.Parse("a0000000-0000-0000-0000-000000000002");

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = adminRoleId, Name = "admin", Description = "管理员" },
            new Role { Id = memberRoleId, Name = "member", Description = "普通成员" }
        );

        modelBuilder.Entity<Permission>().HasData(
            new Permission { Id = Guid.Parse("b0000000-0000-0000-0000-000000000001"), Code = "content:write", Description = "编辑内容" },
            new Permission { Id = Guid.Parse("b0000000-0000-0000-0000-000000000002"), Code = "content:review", Description = "审核内容" },
            new Permission { Id = Guid.Parse("b0000000-0000-0000-0000-000000000003"), Code = "users:manage", Description = "管理用户" },
            new Permission { Id = Guid.Parse("b0000000-0000-0000-0000-000000000004"), Code = "content:view", Description = "查看内容" }
        );

        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission { RoleId = adminRoleId, PermissionId = Guid.Parse("b0000000-0000-0000-0000-000000000001") },
            new RolePermission { RoleId = adminRoleId, PermissionId = Guid.Parse("b0000000-0000-0000-0000-000000000002") },
            new RolePermission { RoleId = adminRoleId, PermissionId = Guid.Parse("b0000000-0000-0000-0000-000000000003") },
            new RolePermission { RoleId = adminRoleId, PermissionId = Guid.Parse("b0000000-0000-0000-0000-000000000004") },
            new RolePermission { RoleId = memberRoleId, PermissionId = Guid.Parse("b0000000-0000-0000-0000-000000000004") }
        );
    }
}
```

- [ ] **Step 2: Create UserRepository**

File: `src/Services/Identity/SkillPlatform.Identity.Infra/Repositories/UserRepository.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using SkillPlatform.Identity.Core.Entities;
using SkillPlatform.Identity.Core.Interfaces;
using SkillPlatform.Identity.Infra.Data;

namespace SkillPlatform.Identity.Infra.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _context;

    public UserRepository(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email.ToLower());
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email.ToLower());
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Role>> GetUserRolesAsync(Guid userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .Select(ur => ur.Role)
            .ToListAsync();
    }

    public async Task<List<Permission>> GetUserPermissionsAsync(Guid userId)
    {
        var roleIds = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        return await _context.RolePermissions
            .Where(rp => roleIds.Contains(rp.RoleId))
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission)
            .ToListAsync();
    }

    public async Task<Role?> GetRoleByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
    }
}
```

- [ ] **Step 3: Build Infra project**

```bash
dotnet build src/Services/Identity/SkillPlatform.Identity.Infra/SkillPlatform.Identity.Infra.csproj
```

Expected: Build succeeds.

- [ ] **Step 4: Commit**

```bash
git add src/Services/Identity/SkillPlatform.Identity.Infra/
git commit -m "feat: add Identity.Infra with DbContext and UserRepository

- IdentityDbContext with EF Core configuration for 5 tables
- Seed data: admin/member roles, content/view/manage permissions
- UserRepository implementing IUserRepository with full queries

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

## Task 5: Identity.Api Web API 层 (Controllers + Program.cs)

**Files:**
- Create: `src/Services/Identity/SkillPlatform.Identity.Api/Controllers/AuthController.cs`
- Create: `src/Services/Identity/SkillPlatform.Identity.Api/Controllers/UsersController.cs`
- Modify: `src/Services/Identity/SkillPlatform.Identity.Api/Program.cs`
- Modify: `src/Services/Identity/SkillPlatform.Identity.Api/appsettings.json`

- [ ] **Step 1: Create appsettings.json**

File: `src/Services/Identity/SkillPlatform.Identity.Api/appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=identity;Username=skilldev;Password=SkillDev@2026"
  },
  "Jwt": {
    "SecretKey": "SkillPlatform@2026!SuperSecretKey!!MinLength32",
    "Issuer": "SkillPlatform",
    "Audience": "SkillPlatform",
    "ExpiresInMinutes": "60"
  },
  "AllowedHosts": "*"
}
```

- [ ] **Step 2: Create AuthController**

File: `src/Services/Identity/SkillPlatform.Identity.Api/Controllers/AuthController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using SkillPlatform.Identity.Core.DTOs;
using SkillPlatform.Identity.Core.Interfaces;

namespace SkillPlatform.Identity.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _authService.LoginAsync(request);
        _logger.LogInformation("用户 {Email} 登录成功", request.Email);
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _authService.RegisterAsync(request);
        _logger.LogInformation("新用户注册: {Email}", request.Email);
        return CreatedAtAction(nameof(Register), response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var response = await _authService.RefreshTokenAsync(request.RefreshToken);
        return Ok(response);
    }
}

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
```

- [ ] **Step 3: Create UsersController**

File: `src/Services/Identity/SkillPlatform.Identity.Api/Controllers/UsersController.cs`

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillPlatform.Identity.Core.DTOs;
using SkillPlatform.Identity.Core.Interfaces;

namespace SkillPlatform.Identity.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "admin")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok("用户列表将在 P2 中完善分页和筛选");
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return NotFound();

        var roles = await _userRepository.GetUserRolesAsync(userId);

        var dto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Avatar = user.Avatar,
            Title = user.Title,
            Department = user.Department,
            Roles = roles.Select(r => r.Name).ToList()
        };

        return Ok(dto);
    }
}
```

- [ ] **Step 4: Create Program.cs**

File: `src/Services/Identity/SkillPlatform.Identity.Api/Program.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using SkillPlatform.Common.Auth;
using SkillPlatform.Common.Middleware;
using SkillPlatform.Identity.Core.Interfaces;
using SkillPlatform.Identity.Core.Services;
using SkillPlatform.Identity.Infra.Data;
using SkillPlatform.Identity.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Auth
builder.Services.AddJwtAuthentication(
    secretKey: builder.Configuration["Jwt:SecretKey"]!,
    issuer: builder.Configuration["Jwt:Issuer"]!,
    audience: builder.Configuration["Jwt:Audience"]!
);

// DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (开发阶段允许前端跨域)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Auto migrate on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    db.Database.Migrate();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

- [ ] **Step 5: Add EF Core tools and create initial migration**

```bash
dotnet add src/Services/Identity/SkillPlatform.Identity.Api/SkillPlatform.Identity.Api.csproj package Microsoft.EntityFrameworkCore.Design
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project src/Services/Identity/SkillPlatform.Identity.Infra/SkillPlatform.Identity.Infra.csproj --startup-project src/Services/Identity/SkillPlatform.Identity.Api/SkillPlatform.Identity.Api.csproj
```

- [ ] **Step 6: Build API project**

```bash
dotnet build src/Services/Identity/SkillPlatform.Identity.Api/SkillPlatform.Identity.Api.csproj
```

Expected: Build succeeds.

- [ ] **Step 7: Commit**

```bash
git add src/Services/Identity/SkillPlatform.Identity.Api/ src/Services/Identity/SkillPlatform.Identity.Infra/Migrations/
git commit -m "feat: add Identity.Api with auth and user controllers

- AuthController: POST /api/auth/login, /register, /refresh
- UsersController: GET /api/users, GET /api/users/me
- Program.cs with EF Core, JWT, CORS, auto-migration
- Initial EF Core migration (InitialCreate)

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

## Task 6: Identity.Tests 单元测试

**Files:**
- Create: `src/Services/Identity/SkillPlatform.Identity.Tests/AuthServiceTests.cs`

- [ ] **Step 1: Write AuthService tests**

File: `src/Services/Identity/SkillPlatform.Identity.Tests/AuthServiceTests.cs`

```csharp
using Microsoft.Extensions.Configuration;
using Moq;
using SkillPlatform.Identity.Core.DTOs;
using SkillPlatform.Identity.Core.Entities;
using SkillPlatform.Identity.Core.Interfaces;
using SkillPlatform.Identity.Core.Services;

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
        // Arrange
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

        // Act
        var result = await _sut.RegisterAsync(request);

        // Assert
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
        // Arrange
        _repoMock.Setup(r => r.EmailExistsAsync("exists@test.com")).ReturnsAsync(true);

        var request = new RegisterRequest
        {
            Username = "existing",
            Email = "exists@test.com",
            Password = "Test123!"
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.RegisterAsync(request));
        Assert.Equal("该邮箱已注册", ex.Message);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsAuthResponse()
    {
        // Arrange
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

        // Act
        var result = await _sut.LoginAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.AccessToken);
        Assert.Equal("testuser", result.User.Username);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ThrowsInvalidOperation()
    {
        // Arrange
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

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.LoginAsync(request));
        Assert.Equal("邮箱或密码错误", ex.Message);
    }

    [Fact]
    public async Task Login_WithNonExistentEmail_ThrowsInvalidOperation()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByEmailAsync("nobody@test.com")).ReturnsAsync((User?)null);

        var request = new LoginRequest
        {
            Email = "nobody@test.com",
            Password = "Anything"
        };

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.LoginAsync(request));
        Assert.Equal("邮箱或密码错误", ex.Message);
    }
}
```

- [ ] **Step 2: Run tests**

```bash
dotnet test src/Services/Identity/SkillPlatform.Identity.Tests/SkillPlatform.Identity.Tests.csproj -v normal
```

Expected: 5 tests pass, 0 fail.

- [ ] **Step 3: Commit**

```bash
git add src/Services/Identity/SkillPlatform.Identity.Tests/
git commit -m "test: add AuthService unit tests (5 tests)

- Register with new email returns auth response
- Register with existing email throws
- Login with valid credentials returns token
- Login with wrong password throws
- Login with non-existent email throws

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

## Task 7: YARP 网关配置

**Files:**
- Modify: `src/Gateway/SkillPlatform.Gateway/Program.cs`
- Modify: `src/Gateway/SkillPlatform.Gateway/appsettings.json`

- [ ] **Step 1: Configure Gateway Program.cs**

File: `src/Gateway/SkillPlatform.Gateway/Program.cs`

```csharp
using SkillPlatform.Common.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors();
app.MapReverseProxy();

app.Run();
```

- [ ] **Step 2: Configure Gateway appsettings.json**

File: `src/Gateway/SkillPlatform.Gateway/appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ReverseProxy": {
    "Routes": {
      "identity-route": {
        "ClusterId": "identity-cluster",
        "Match": {
          "Path": "/api/auth/{**catch-all}"
        }
      },
      "users-route": {
        "ClusterId": "identity-cluster",
        "Match": {
          "Path": "/api/users/{**catch-all}"
        }
      }
    },
    "Clusters": {
      "identity-cluster": {
        "Destinations": {
          "identity-destination": {
            "Address": "http://localhost:5001/"
          }
        }
      }
    }
  }
}
```

- [ ] **Step 3: Add Common reference to Gateway**

Run:
```bash
dotnet add src/Gateway/SkillPlatform.Gateway/SkillPlatform.Gateway.csproj reference src/Shared/SkillPlatform.Common/SkillPlatform.Common.csproj
```

- [ ] **Step 4: Build Gateway**

```bash
dotnet build src/Gateway/SkillPlatform.Gateway/SkillPlatform.Gateway.csproj
```

Expected: Build succeeds.

- [ ] **Step 5: Commit**

```bash
git add src/Gateway/
git commit -m "feat: configure YARP gateway with Identity routes

- Routes /api/auth/* and /api/users/* to Identity service
- CORS for Vue dev server
- Exception middleware from Common package

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

## Task 8: Vue 3 前端脚手架 (Login + Register + Dashboard)

**Files:**
- Create: `client/skill-platform-web/package.json`
- Create: `client/skill-platform-web/vite.config.ts`
- Create: `client/skill-platform-web/index.html`
- Create: `client/skill-platform-web/src/main.ts`
- Create: `client/skill-platform-web/src/App.vue`
- Create: `client/skill-platform-web/src/router/index.ts`
- Create: `client/skill-platform-web/src/stores/auth.ts`
- Create: `client/skill-platform-web/src/api/http.ts`
- Create: `client/skill-platform-web/src/api/auth.ts`
- Create: `client/skill-platform-web/src/views/LoginView.vue`
- Create: `client/skill-platform-web/src/views/RegisterView.vue`
- Create: `client/skill-platform-web/src/views/DashboardView.vue`
- Create: `client/skill-platform-web/src/components/AppHeader.vue`
- Create: `client/skill-platform-web/src/style.css`
- Create: `client/skill-platform-web/tsconfig.json`
- Create: `client/skill-platform-web/tsconfig.node.json`
- Create: `client/skill-platform-web/env.d.ts`

- [ ] **Step 1: Scaffold Vue 3 project**

```bash
cd client
npm create vite@latest skill-platform-web -- --template vue-ts
cd skill-platform-web
npm install
npm install pinia vue-router axios
npm install -D @types/node
```

- [ ] **Step 2: Create HttpClient wrapper**

File: `client/skill-platform-web/src/api/http.ts`

```typescript
import axios from 'axios';
import type { AxiosInstance, AxiosRequestConfig, AxiosError } from 'axios';

const baseURL = 'http://localhost:8080';

const http: AxiosInstance = axios.create({
  baseURL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

http.interceptors.request.use((config) => {
  const token = localStorage.getItem('accessToken');
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

http.interceptors.response.use(
  (response) => response,
  (error: AxiosError<{ code?: string; message?: string }>) => {
    const message = error.response?.data?.message || error.message || '请求失败';
    console.error(`[API Error] ${message}`);

    if (error.response?.status === 401) {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }

    return Promise.reject(error);
  }
);

export default http;
```

- [ ] **Step 3: Create auth API module**

File: `client/skill-platform-web/src/api/auth.ts`

```typescript
import http from './http';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  department?: string;
  title?: string;
}

export interface UserInfo {
  id: string;
  username: string;
  email: string;
  avatar: string | null;
  title: string | null;
  department: string | null;
  roles: string[];
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  user: UserInfo;
}

export const authApi = {
  login(data: LoginRequest): Promise<AuthResponse> {
    return http.post('/api/auth/login', data).then((res) => res.data);
  },

  register(data: RegisterRequest): Promise<AuthResponse> {
    return http.post('/api/auth/register', data).then((res) => res.data);
  },

  getCurrentUser(): Promise<UserInfo> {
    return http.get('/api/users/me').then((res) => res.data);
  },
};
```

- [ ] **Step 4: Create Pinia auth store**

File: `client/skill-platform-web/src/stores/auth.ts`

```typescript
import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import { authApi } from '@/api/auth';
import type { UserInfo, LoginRequest, RegisterRequest } from '@/api/auth';

export const useAuthStore = defineStore('auth', () => {
  const user = ref<UserInfo | null>(
    JSON.parse(localStorage.getItem('user') || 'null')
  );
  const accessToken = ref<string | null>(localStorage.getItem('accessToken'));
  const refreshToken = ref<string | null>(localStorage.getItem('refreshToken'));

  const isLoggedIn = computed(() => !!accessToken.value);
  const isAdmin = computed(() => user.value?.roles?.includes('admin') ?? false);

  async function login(data: LoginRequest) {
    const response = await authApi.login(data);
    setSession(response);
  }

  async function register(data: RegisterRequest) {
    const response = await authApi.register(data);
    setSession(response);
  }

  function setSession(response: { accessToken: string; refreshToken: string; user: UserInfo }) {
    accessToken.value = response.accessToken;
    refreshToken.value = response.refreshToken;
    user.value = response.user;

    localStorage.setItem('accessToken', response.accessToken);
    localStorage.setItem('refreshToken', response.refreshToken);
    localStorage.setItem('user', JSON.stringify(response.user));
  }

  function logout() {
    accessToken.value = null;
    refreshToken.value = null;
    user.value = null;

    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
  }

  return { user, accessToken, isLoggedIn, isAdmin, login, register, logout };
});
```

- [ ] **Step 5: Create router**

File: `client/skill-platform-web/src/router/index.ts`

```typescript
import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'dashboard',
      component: () => import('@/views/DashboardView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/LoginView.vue'),
      meta: { guest: true },
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('@/views/RegisterView.vue'),
      meta: { guest: true },
    },
  ],
});

router.beforeEach((to, _from, next) => {
  const auth = useAuthStore();
  if (to.meta.requiresAuth && !auth.isLoggedIn) {
    next('/login');
  } else if (to.meta.guest && auth.isLoggedIn) {
    next('/');
  } else {
    next();
  }
});

export default router;
```

- [ ] **Step 6: Create views**

File: `client/skill-platform-web/src/views/LoginView.vue`

```vue
<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const router = useRouter();
const auth = useAuthStore();

const email = ref('');
const password = ref('');
const error = ref('');
const loading = ref(false);

async function handleLogin() {
  error.value = '';
  loading.value = true;
  try {
    await auth.login({ email: email.value, password: password.value });
    router.push('/');
  } catch (e: any) {
    error.value = e.response?.data?.message || '登录失败，请稍后重试';
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <div class="auth-container">
    <div class="auth-card">
      <h1>技术能力提升平台</h1>
      <h2>欢迎回来</h2>

      <form @submit.prevent="handleLogin">
        <div class="form-group">
          <label for="email">邮箱</label>
          <input
            id="email"
            v-model="email"
            type="email"
            placeholder="请输入邮箱"
            required
            autocomplete="email"
          />
        </div>

        <div class="form-group">
          <label for="password">密码</label>
          <input
            id="password"
            v-model="password"
            type="password"
            placeholder="请输入密码"
            required
            autocomplete="current-password"
          />
        </div>

        <p v-if="error" class="error-message">{{ error }}</p>

        <button type="submit" :disabled="loading" class="btn-primary">
          {{ loading ? '登录中...' : '登录' }}
        </button>
      </form>

      <p class="auth-link">
        还没有账号？<router-link to="/register">立即注册</router-link>
      </p>
    </div>
  </div>
</template>
```

File: `client/skill-platform-web/src/views/RegisterView.vue`

```vue
<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const router = useRouter();
const auth = useAuthStore();

const username = ref('');
const email = ref('');
const password = ref('');
const department = ref('');
const error = ref('');
const loading = ref(false);

async function handleRegister() {
  error.value = '';
  if (password.value.length < 6) {
    error.value = '密码至少6位';
    return;
  }
  loading.value = true;
  try {
    await auth.register({
      username: username.value,
      email: email.value,
      password: password.value,
      department: department.value || undefined,
    });
    router.push('/');
  } catch (e: any) {
    error.value = e.response?.data?.message || '注册失败，请稍后重试';
  } finally {
    loading.value = false;
  }
}
</script>

<template>
  <div class="auth-container">
    <div class="auth-card">
      <h1>技术能力提升平台</h1>
      <h2>创建账号</h2>

      <form @submit.prevent="handleRegister">
        <div class="form-group">
          <label for="username">用户名</label>
          <input
            id="username"
            v-model="username"
            type="text"
            placeholder="请输入用户名"
            required
          />
        </div>

        <div class="form-group">
          <label for="email">邮箱</label>
          <input
            id="email"
            v-model="email"
            type="email"
            placeholder="请输入邮箱"
            required
          />
        </div>

        <div class="form-group">
          <label for="password">密码</label>
          <input
            id="password"
            v-model="password"
            type="password"
            placeholder="至少6位密码"
            required
          />
        </div>

        <div class="form-group">
          <label for="department">部门（选填）</label>
          <input
            id="department"
            v-model="department"
            type="text"
            placeholder="如：后端开发部"
          />
        </div>

        <p v-if="error" class="error-message">{{ error }}</p>

        <button type="submit" :disabled="loading" class="btn-primary">
          {{ loading ? '注册中...' : '注册' }}
        </button>
      </form>

      <p class="auth-link">
        已有账号？<router-link to="/login">去登录</router-link>
      </p>
    </div>
  </div>
</template>
```

File: `client/skill-platform-web/src/views/DashboardView.vue`

```vue
<script setup lang="ts">
import { useAuthStore } from '@/stores/auth';
import { useRouter } from 'vue-router';

const auth = useAuthStore();
const router = useRouter();

function handleLogout() {
  auth.logout();
  router.push('/login');
}
</script>

<template>
  <div class="dashboard">
    <header class="app-header">
      <h1>技术能力提升平台</h1>
      <div class="user-info">
        <span v-if="auth.user">你好，{{ auth.user.username }}</span>
        <button @click="handleLogout" class="btn-link">退出</button>
      </div>
    </header>

    <main class="dashboard-main">
      <div class="welcome-card">
        <h2>技术能力提升平台</h2>
        <p>功能模块将在 P2~P4 阶段逐步建设。当前 P1 骨架已就绪。</p>
      </div>

      <div class="tech-stack-grid">
        <div class="tech-card">
          <h3>.NET 6 微服务</h3>
          <p>ASP.NET Core, EF Core, YARP</p>
        </div>
        <div class="tech-card">
          <h3>数据存储</h3>
          <p>PostgreSQL, Redis, RabbitMQ</p>
        </div>
        <div class="tech-card">
          <h3>前端框架</h3>
          <p>Vue 3 + Pinia + Vite</p>
        </div>
        <div class="tech-card">
          <h3>业务领域</h3>
          <p>SaaS · IoT · APS · 数字孪生</p>
        </div>
      </div>
    </main>
  </div>
</template>
```

- [ ] **Step 7: Create App.vue**

File: `client/skill-platform-web/src/App.vue`

```vue
<template>
  <router-view />
</template>
```

- [ ] **Step 8: Create main.ts**

File: `client/skill-platform-web/src/main.ts`

```typescript
import { createApp } from 'vue';
import { createPinia } from 'pinia';
import router from './router';
import App from './App.vue';
import './style.css';

const app = createApp(App);
app.use(createPinia());
app.use(router);
app.mount('#app');
```

- [ ] **Step 9: Create global styles**

File: `client/skill-platform-web/src/style.css`

```css
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  background: #f0f2f5;
  color: #333;
  min-height: 100vh;
}

.auth-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  padding: 20px;
}

.auth-card {
  background: #fff;
  border-radius: 12px;
  padding: 40px;
  width: 100%;
  max-width: 420px;
  box-shadow: 0 2px 16px rgba(0, 0, 0, 0.08);
}

.auth-card h1 {
  font-size: 20px;
  color: #1a73e8;
  text-align: center;
  margin-bottom: 8px;
}

.auth-card h2 {
  font-size: 16px;
  font-weight: 400;
  color: #666;
  text-align: center;
  margin-bottom: 32px;
}

.form-group {
  margin-bottom: 20px;
}

.form-group label {
  display: block;
  margin-bottom: 6px;
  font-size: 14px;
  font-weight: 500;
  color: #444;
}

.form-group input {
  width: 100%;
  padding: 10px 14px;
  border: 1px solid #d9d9d9;
  border-radius: 8px;
  font-size: 14px;
  transition: border-color 0.2s;
}

.form-group input:focus {
  outline: none;
  border-color: #1a73e8;
  box-shadow: 0 0 0 2px rgba(26, 115, 232, 0.12);
}

.btn-primary {
  width: 100%;
  padding: 12px;
  background: #1a73e8;
  color: #fff;
  border: none;
  border-radius: 8px;
  font-size: 15px;
  font-weight: 500;
  cursor: pointer;
  transition: background 0.2s;
}

.btn-primary:hover {
  background: #1557b0;
}

.btn-primary:disabled {
  background: #93b8f0;
  cursor: not-allowed;
}

.error-message {
  color: #d93025;
  font-size: 13px;
  margin-bottom: 16px;
  padding: 8px 12px;
  background: #fce8e6;
  border-radius: 6px;
}

.auth-link {
  text-align: center;
  margin-top: 20px;
  font-size: 14px;
  color: #666;
}

.auth-link a {
  color: #1a73e8;
  text-decoration: none;
  font-weight: 500;
}

.auth-link a:hover {
  text-decoration: underline;
}

.dashboard {
  min-height: 100vh;
}

.app-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px 32px;
  background: #fff;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.06);
}

.app-header h1 {
  font-size: 18px;
  color: #1a73e8;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 16px;
  font-size: 14px;
}

.btn-link {
  background: none;
  border: 1px solid #d9d9d9;
  padding: 6px 16px;
  border-radius: 6px;
  cursor: pointer;
  font-size: 13px;
  color: #666;
}

.btn-link:hover {
  background: #f5f5f5;
}

.dashboard-main {
  max-width: 960px;
  margin: 40px auto;
  padding: 0 24px;
}

.welcome-card {
  background: linear-gradient(135deg, #1a73e8 0%, #0d47a1 100%);
  color: #fff;
  border-radius: 12px;
  padding: 40px;
  margin-bottom: 32px;
}

.welcome-card h2 {
  font-size: 24px;
  margin-bottom: 12px;
}

.welcome-card p {
  font-size: 14px;
  opacity: 0.85;
}

.tech-stack-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 20px;
}

.tech-card {
  background: #fff;
  border-radius: 10px;
  padding: 24px;
  box-shadow: 0 1px 6px rgba(0, 0, 0, 0.06);
  transition: box-shadow 0.2s;
}

.tech-card:hover {
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.1);
}

.tech-card h3 {
  font-size: 15px;
  color: #1a73e8;
  margin-bottom: 8px;
}

.tech-card p {
  font-size: 13px;
  color: #888;
}
```

- [ ] **Step 10: Verify frontend build**

```bash
cd client/skill-platform-web
npm run build
```

Expected: Build succeeds, no errors.

- [ ] **Step 11: Commit**

```bash
git add client/
git commit -m "feat: scaffold Vue 3 frontend with auth flow

- Login/Register views with form validation
- Pinia auth store with localStorage persistence
- Axios HTTP client with interceptors (JWT attach, 401 redirect)
- Vue Router with auth guards
- Dashboard placeholder showing tech stack cards
- Clean responsive CSS

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

## Task 9: 集成验证 — 从 Docker Compose 启动并测试全流程

- [ ] **Step 1: Start PostgreSQL**

```bash
docker compose -f docker/docker-compose.dev.yml up -d
```

Expected: PostgreSQL container running on port 5432.

- [ ] **Step 2: Set Identity API on fixed port**

Create `src/Services/Identity/SkillPlatform.Identity.Api/Properties/launchSettings.json` (先 `mkdir -p src/Services/Identity/SkillPlatform.Identity.Api/Properties`):

```json
{
  "profiles": {
    "SkillPlatform.Identity.Api": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "http://localhost:5001",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

- [ ] **Step 3: Start Identity API**

```bash
dotnet run --project src/Services/Identity/SkillPlatform.Identity.Api/SkillPlatform.Identity.Api.csproj
```

Wait for: `Now listening on: http://localhost:5001`

- [ ] **Step 4: Start Gateway (in a separate terminal)**

```bash
dotnet run --project src/Gateway/SkillPlatform.Gateway/SkillPlatform.Gateway.csproj
```

Wait for: Gateway running.

- [ ] **Step 5: Test Register API**

```bash
curl -s -X POST http://localhost:8080/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "张三",
    "email": "zhangsan@company.com",
    "password": "Test@123",
    "department": "后端开发部",
    "title": "高级工程师"
  }' | python -m json.tool
```

Expected: 201 Created with JSON containing accessToken, refreshToken, user object with roles=["member"].

- [ ] **Step 6: Test Login API**

```bash
curl -s -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "zhangsan@company.com",
    "password": "Test@123"
  }' | python -m json.tool
```

Expected: 200 OK with accessToken and user info.

- [ ] **Step 7: Test protected endpoint with token**

```bash
# Copy token from Step 6 response
export TOKEN="<accessToken from login response>"
curl -s http://localhost:8080/api/users/me \
  -H "Authorization: Bearer $TOKEN" | python -m json.tool
```

Expected: 200 OK with user info matching the registered user.

- [ ] **Step 8: Test unauthorized access**

```bash
curl -s -X GET http://localhost:8080/api/users/me
```

Expected: 401 with error message "请先登录".

- [ ] **Step 9: Start Vue dev server and verify UI flow**

```bash
cd client/skill-platform-web
npm run dev
```

Manual verification:
1. Open http://localhost:5173 → 应自动跳转到 /login
2. 点击"立即注册" → 填入信息 → 注册成功 → 跳转到 Dashboard
3. 刷新页面 → 仍保持登录状态
4. 退出 → 回到登录页
5. 用刚注册的账号登录 → 进入 Dashboard

- [ ] **Step 10: Run full test suite**

```bash
dotnet test
```

Expected: All tests pass.

- [ ] **Step 11: Final commit**

```bash
git add -A
git commit -m "verify: P1 integration tests pass — register, login, JWT, frontend

- End-to-end auth flow verified via Gateway (.NET) and Vue dev server
- Protected endpoint returns 401 without token
- All xUnit tests green

Co-Authored-By: Claude Opus 4.8 <noreply@anthropic.com>"
```

---

## 验证清单 (P1 完成标准)

- [x] 解决方案包含 6 个项目，全部通过编译
- [x] PostgreSQL 通过 Docker Compose 启动
- [x] Identity API 启动成功，EF 自动迁移
- [x] YARP 网关正确路由到 Identity 服务
- [x] 注册接口返回 JWT + 用户信息，角色默认为 member
- [x] 登录接口验证凭据，返回 JWT
- [x] GET /api/users/me 在无 Token 时返回 401
- [x] GET /api/users/me 携带有效 Token 时返回用户信息
- [x] AuthService 单元测试 5 个用例全部通过
- [x] Vue 3 前端构建成功，登录/注册/Dashboard 流程跑通
