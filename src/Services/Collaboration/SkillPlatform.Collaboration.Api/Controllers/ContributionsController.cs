using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillPlatform.Collaboration.Core.DTOs;
using SkillPlatform.Collaboration.Core.Interfaces;

namespace SkillPlatform.Collaboration.Api.Controllers;

[ApiController]
[Route("api/contributions")]
[Authorize]
public class ContributionsController : ControllerBase
{
    private readonly ICollaborationRepository _repo;
    public ContributionsController(ICollaborationRepository repo) => _repo = repo;

    private Guid GetUserId()
    {
        var c = User.FindFirst(ClaimTypes.NameIdentifier);
        return c != null ? Guid.Parse(c.Value) : Guid.Empty;
    }

    [HttpPost]
    public async Task<IActionResult> Submit([FromBody] ContributionDto dto)
    {
        var c = await _repo.SubmitContributionAsync(new Core.Entities.Contribution
        {
            Id = Guid.NewGuid(), UserId = GetUserId(), Type = dto.Type, Target = dto.Target
        });
        return Ok(new ContributionDto { Id = c.Id, UserId = c.UserId, Type = c.Type, Target = c.Target, Status = c.Status });
    }

    [HttpGet("pending")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetPending()
    {
        var items = await _repo.GetPendingContributionsAsync();
        var dtos = items.Select(c => new ContributionDto { Id = c.Id, UserId = c.UserId, Type = c.Type, Target = c.Target, Status = c.Status, CreatedAt = c.CreatedAt });
        return Ok(dtos);
    }

    [HttpPut("{id:guid}/review")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Review(Guid id, [FromBody] ReviewRequest req)
    {
        var c = await _repo.ReviewContributionAsync(id, req.Status, GetUserId(), req.Comment);
        return Ok(new ContributionDto { Id = c.Id, UserId = c.UserId, Type = c.Type, Target = c.Target, Status = c.Status });
    }
}
