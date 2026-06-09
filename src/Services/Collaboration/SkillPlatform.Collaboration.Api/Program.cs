using Microsoft.EntityFrameworkCore;
using SkillPlatform.Collaboration.Core.Interfaces;
using SkillPlatform.Collaboration.Infra.Data;
using SkillPlatform.Collaboration.Infra.Repositories;
using SkillPlatform.Common.Auth;
using SkillPlatform.Common.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CollaborationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddJwtAuthentication(
    builder.Configuration["Jwt:SecretKey"]!,
    builder.Configuration["Jwt:Issuer"]!,
    builder.Configuration["Jwt:Audience"]!);

builder.Services.AddScoped<ICollaborationRepository, CollaborationRepository>();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CollaborationDbContext>();
    db.Database.Migrate();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors();
app.UseSwagger(); app.UseSwaggerUI();
app.UseAuthentication(); app.UseAuthorization();
app.MapControllers();
app.Run();
