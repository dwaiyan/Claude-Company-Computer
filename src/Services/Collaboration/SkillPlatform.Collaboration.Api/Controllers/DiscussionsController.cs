using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillPlatform.Collaboration.Core.DTOs;
using SkillPlatform.Collaboration.Core.Interfaces;

namespace SkillPlatform.Collaboration.Api.Controllers;

[ApiController]
[Route("api/discussions")]
[Authorize]
public class DiscussionsController : ControllerBase
{
    private readonly ICollaborationRepository _repo;
    public DiscussionsController(ICollaborationRepository repo) => _repo = repo;

    private Guid GetUserId()
    {
        var c = User.FindFirst(ClaimTypes.NameIdentifier);
        return c != null ? Guid.Parse(c.Value) : Guid.Empty;
    }

    [HttpGet]
    public async Task<IActionResult> GetDiscussions([FromQuery] Guid node)
    {
        var discussions = await _repo.GetDiscussionsByNodeAsync(node);
        var dtos = discussions.Select(MapDiscussion).ToList();
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<IActionResult> AddDiscussion([FromBody] CreateDiscussionRequest req)
    {
        var d = await _repo.AddDiscussionAsync(new Core.Entities.Discussion
        {
            Id = Guid.NewGuid(), NodeId = req.NodeId, UserId = GetUserId(),
            Content = req.Content, ParentId = req.ParentId
        });
        return Ok(MapDiscussion(d));
    }

    private static DiscussionDto MapDiscussion(Core.Entities.Discussion d) => new()
    {
        Id = d.Id, NodeId = d.NodeId, UserId = d.UserId, Content = d.Content,
        ParentId = d.ParentId, CreatedAt = d.CreatedAt,
        Replies = d.Replies.Select(MapDiscussion).ToList()
    };
}
