using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace SkillPlatform.Notification.Api.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IConnectionMultiplexer _redis;

    public NotificationsController(IConnectionMultiplexer redis) => _redis = redis;

    private Guid GetUserId()
    {
        var c = User.FindFirst(ClaimTypes.NameIdentifier);
        return c != null ? Guid.Parse(c.Value) : Guid.Empty;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var db = _redis.GetDatabase();
        var key = $"notifications:{GetUserId()}";
        var items = await db.ListRangeAsync(key, 0, 49);

        var notifications = items.Select(i =>
            JsonSerializer.Deserialize<NotificationDto>(i!)
        ).Where(n => n != null).ToList();

        return Ok(notifications);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        var db = _redis.GetDatabase();
        var key = $"notifications:{GetUserId()}";
        var items = await db.ListRangeAsync(key);
        foreach (var item in items)
        {
            var n = JsonSerializer.Deserialize<NotificationDto>(item!);
            if (n?.Id == id)
            {
                n.IsRead = true;
                // Update by removing and re-inserting at same position is complex with Redis List
                // Simplified: just mark the whole notification
                await db.ListRemoveAsync(key, item);
                await db.ListLeftPushAsync(key, JsonSerializer.Serialize(n));
                break;
            }
        }
        return Ok();
    }

    [HttpPost("test")]
    public async Task<IActionResult> AddTestNotification()
    {
        var db = _redis.GetDatabase();
        var notification = new NotificationDto
        {
            Id = Guid.NewGuid().ToString(),
            Type = "system",
            Title = "测试通知",
            Body = "这是一条测试通知",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
        await db.ListLeftPushAsync($"notifications:{GetUserId()}", JsonSerializer.Serialize(notification));
        return Ok(notification);
    }
}

public class NotificationDto
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
