using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillPlatform.Learning.Core.Entities;
using SkillPlatform.Learning.Infra.Data;

namespace SkillPlatform.Learning.Api.Controllers;

[ApiController]
[Route("api/admin/assessments")]
[Authorize(Roles = "admin")]
public class AdminAssessmentsController : ControllerBase
{
    private readonly LearningDbContext _context;

    public AdminAssessmentsController(LearningDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var assessments = await _context.Assessments.OrderByDescending(a => a.CreatedAt).ToListAsync();
        return Ok(assessments.Select(MapToDto));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAssessmentRequest req)
    {
        var assessment = new Assessment
        {
            Id = Guid.NewGuid(),
            Title = req.Title,
            Type = req.Type,
            NodeId = req.NodeId,
            QuestionsJson = JsonSerializer.Serialize(req.Questions),
            TimeLimit = req.TimeLimit,
            PassScore = req.PassScore
        };
        _context.Assessments.Add(assessment);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), MapToDto(assessment));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateAssessmentRequest req)
    {
        var assessment = await _context.Assessments.FindAsync(id);
        if (assessment == null) return NotFound();
        assessment.Title = req.Title; assessment.Type = req.Type;
        assessment.NodeId = req.NodeId; assessment.TimeLimit = req.TimeLimit;
        assessment.PassScore = req.PassScore;
        assessment.QuestionsJson = JsonSerializer.Serialize(req.Questions);
        await _context.SaveChangesAsync();
        return Ok(MapToDto(assessment));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var assessment = await _context.Assessments.FindAsync(id);
        if (assessment == null) return NotFound();
        _context.Assessments.Remove(assessment);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var total = await _context.Assessments.CountAsync();
        var totalRecords = await _context.AssessmentRecords.CountAsync();
        var avgScore = await _context.AssessmentRecords.AnyAsync()
            ? await _context.AssessmentRecords.AverageAsync(r => (double?)r.Score) ?? 0
            : 0;
        return Ok(new { total, totalRecords, avgScore = Math.Round(avgScore, 1) });
    }

    private static object MapToDto(Assessment a) => new
    {
        a.Id, a.Title, a.Type, a.NodeId, a.TimeLimit, a.PassScore,
        Questions = JsonSerializer.Deserialize<object>(a.QuestionsJson),
        a.CreatedAt
    };
}

public class CreateAssessmentRequest
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = "choice";
    public Guid NodeId { get; set; }
    public List<QuestionItem> Questions { get; set; } = new();
    public int TimeLimit { get; set; } = 30;
    public int PassScore { get; set; } = 60;
}

public class QuestionItem
{
    public string Text { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    public int CorrectIndex { get; set; }
    public string? ExpectedOutput { get; set; } // for code questions
}
