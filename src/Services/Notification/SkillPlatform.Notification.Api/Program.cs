using SkillPlatform.Common.Auth;
using SkillPlatform.Common.Middleware;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));

builder.Services.AddJwtAuthentication(
    builder.Configuration["Jwt:SecretKey"]!,
    builder.Configuration["Jwt:Issuer"]!,
    builder.Configuration["Jwt:Audience"]!);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors();
app.UseSwagger(); app.UseSwaggerUI();
app.UseAuthentication(); app.UseAuthorization();
app.MapControllers();
app.Run();
