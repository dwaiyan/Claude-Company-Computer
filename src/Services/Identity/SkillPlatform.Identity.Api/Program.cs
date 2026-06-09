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

// CORS
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
