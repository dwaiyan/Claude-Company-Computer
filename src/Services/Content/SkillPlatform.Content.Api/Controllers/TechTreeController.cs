using Microsoft.AspNetCore.Mvc;
using SkillPlatform.Content.Core.DTOs;
using SkillPlatform.Content.Core.Interfaces;

namespace SkillPlatform.Content.Api.Controllers;

[ApiController]
[Route("api/tech-trees")]
public class TechTreeController : ControllerBase
{
    private readonly ITechTreeRepository _repo;

    public TechTreeController(ITechTreeRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var trees = await _repo.GetAllAsync();
        var dtos = trees.Select(MapTree).ToList();
        return Ok(dtos);
    }

    [HttpGet("nodes")]
    public async Task<IActionResult> GetNodes([FromQuery] Guid tree)
    {
        var nodes = await _repo.GetNodesByTreeAsync(tree);
        var dtos = nodes.Select(MapNode).ToList();
        return Ok(dtos);
    }

    [HttpGet("nodes/{id:guid}")]
    public async Task<IActionResult> GetNode(Guid id)
    {
        var node = await _repo.GetNodeByIdAsync(id);
        if (node == null) return NotFound();
        return Ok(MapNode(node));
    }

    [HttpGet("nodes/{nodeId:guid}/resources")]
    public async Task<IActionResult> GetResources(Guid nodeId)
    {
        var resources = await _repo.GetResourcesByNodeAsync(nodeId);
        var dtos = resources.Select(r => new ResourceDto
        {
            Id = r.Id, Title = r.Title, Url = r.Url, Type = r.Type, Difficulty = r.Difficulty
        }).ToList();
        return Ok(dtos);
    }

    [HttpPost("resources")]
    public async Task<IActionResult> AddResource([FromBody] ResourceDto dto)
    {
        var resource = await _repo.AddResourceAsync(new Core.Entities.Resource
        {
            Id = Guid.NewGuid(),
            NodeId = dto.Id,
            Title = dto.Title,
            Url = dto.Url,
            Type = dto.Type,
            Difficulty = dto.Difficulty,
            CreatedBy = Guid.Empty // TODO: get from JWT
        });
        return CreatedAtAction(nameof(GetResources), new { nodeId = resource.NodeId }, resource);
    }

    [HttpGet("nodes/{nodeId:guid}/questions")]
    public async Task<IActionResult> GetQuestions(Guid nodeId, [FromQuery] int? difficulty)
    {
        var questions = await _repo.GetQuestionsByNodeAsync(nodeId, difficulty);
        var dtos = questions.Select(q => new InterviewQuestionDto
        {
            Id = q.Id, Question = q.Question, AnswerTip = q.AnswerTip,
            Difficulty = q.Difficulty, Category = q.Category
        }).ToList();
        return Ok(dtos);
    }

    private static TechTreeDto MapTree(Core.Entities.TechTree t)
    {
        return new TechTreeDto
        {
            Id = t.Id, Title = t.Title, Description = t.Description, Icon = t.Icon,
            Children = t.Children.Select(MapTree).ToList()
        };
    }

    private static TechNodeDto MapNode(Core.Entities.TechNode n)
    {
        return new TechNodeDto
        {
            Id = n.Id, Title = n.Title, Description = n.Description,
            Level = n.Level, ParentId = n.ParentId,
            Children = n.Children.Select(MapNode).ToList(),
            ResourceCount = n.Resources.Count,
            QuestionCount = n.Questions.Count
        };
    }
}
