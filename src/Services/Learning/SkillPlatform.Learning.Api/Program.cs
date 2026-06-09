using Microsoft.EntityFrameworkCore;
using SkillPlatform.Common.Auth;
using SkillPlatform.Common.Middleware;
using SkillPlatform.Learning.Core.Interfaces;
using SkillPlatform.Learning.Infra.Data;
using SkillPlatform.Learning.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LearningDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddJwtAuthentication(
    secretKey: builder.Configuration["Jwt:SecretKey"]!,
    issuer: builder.Configuration["Jwt:Issuer"]!,
    audience: builder.Configuration["Jwt:Audience"]!
);

builder.Services.AddScoped<ILearningRepository, LearningRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LearningDbContext>();
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
