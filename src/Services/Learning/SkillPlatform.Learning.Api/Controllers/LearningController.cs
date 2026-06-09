using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SkillPlatform.Learning.Core.DTOs;
using SkillPlatform.Learning.Core.Entities;
using SkillPlatform.Learning.Core.Interfaces;

namespace SkillPlatform.Learning.Api.Controllers;

[ApiController]
[Route("api/learning")]
public class LearningController : ControllerBase
{
    private readonly ILearningRepository _repo;

    public LearningController(ILearningRepository repo)
    {
        _repo = repo;
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null ? Guid.Parse(claim.Value) : Guid.Empty;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetUserId();
        var checkIns = await _repo.GetCheckInsByUserAsync(userId);
        var weekAgo = DateTime.UtcNow.AddDays(-7);
        var weeklyMinutes = checkIns.Where(c => c.Date >= weekAgo).Sum(c => c.DurationMinutes);
        var streak = await _repo.GetStreakDaysAsync(userId);
        var assessments = await _repo.GetAssessmentsByUserAsync(userId);

        return Ok(new LearningProfileDto
        {
            UserId = userId,
            StreakDays = streak,
            TotalCheckIns = checkIns.Count,
            WeeklyMinutes = weeklyMinutes,
            SkillRadar = assessments.Select(a => new SkillRadarItem
            {
                NodeId = a.NodeId,
                SelfScore = a.SelfScore,
                Level = a.Level
            }).ToList()
        });
    }

    [HttpPost("check-in")]
    public async Task<IActionResult> CheckIn([FromBody] CheckInRequest request)
    {
        var checkIn = await _repo.AddCheckInAsync(new CheckIn
        {
            Id = Guid.NewGuid(),
            UserId = GetUserId(),
            DurationMinutes = request.DurationMinutes,
            Note = request.Note
        });
        return Ok(new CheckInDto { Id = checkIn.Id, Date = checkIn.Date, DurationMinutes = checkIn.DurationMinutes, Note = checkIn.Note });
    }

    [HttpGet("check-ins")]
    public async Task<IActionResult> GetCheckIns()
    {
        var checkIns = await _repo.GetCheckInsByUserAsync(GetUserId());
        var dtos = checkIns.Select(c => new CheckInDto { Id = c.Id, Date = c.Date, DurationMinutes = c.DurationMinutes, Note = c.Note });
        return Ok(dtos);
    }

    [HttpGet("streak")]
    public async Task<IActionResult> GetStreak()
    {
        var streak = await _repo.GetStreakDaysAsync(GetUserId());
        return Ok(new { streakDays = streak });
    }

    [HttpGet("skill-radar")]
    public async Task<IActionResult> GetSkillRadar()
    {
        var assessments = await _repo.GetAssessmentsByUserAsync(GetUserId());
        var items = assessments.Select(a => new SkillRadarItem { NodeId = a.NodeId, SelfScore = a.SelfScore, Level = a.Level });
        return Ok(items);
    }

    [HttpPut("skill-radar")]
    public async Task<IActionResult> UpdateSkillRadar([FromBody] SkillAssessmentRequest request)
    {
        var assessment = await _repo.UpsertAssessmentAsync(new SkillAssessment
        {
            Id = Guid.NewGuid(),
            UserId = GetUserId(),
            NodeId = request.NodeId,
            SelfScore = request.SelfScore,
            Level = request.Level,
            EvaluatedAt = DateTime.UtcNow
        });
        return Ok(new SkillRadarItem { NodeId = assessment.NodeId, SelfScore = assessment.SelfScore, Level = assessment.Level });
    }

    [HttpGet("assessments")]
    public async Task<IActionResult> GetAssessments([FromQuery] Guid node)
    {
        var assessments = await _repo.GetAssessmentsByNodeAsync(node);
        var dtos = assessments.Select(a => new AssessmentDto
        {
            Id = a.Id, Title = a.Title, Type = a.Type, NodeId = a.NodeId,
            Questions = System.Text.Json.JsonSerializer.Deserialize<object>(a.QuestionsJson)!,
            TimeLimit = a.TimeLimit, PassScore = a.PassScore
        });
        return Ok(dtos);
    }

    [HttpPost("assessments/{id:guid}/submit")]
    public async Task<IActionResult> SubmitAssessment(Guid id, [FromBody] SubmitAssessmentRequest request)
    {
        var assessment = await _repo.GetAssessmentByIdAsync(id);
        if (assessment == null) return NotFound();

        var record = await _repo.SubmitRecordAsync(new AssessmentRecord
        {
            Id = Guid.NewGuid(),
            UserId = GetUserId(),
            AssessmentId = id,
            AnswersJson = System.Text.Json.JsonSerializer.Serialize(request.Answers),
            StartedAt = request.StartedAt
        });
        return Ok(new AssessmentRecordDto
        {
            Id = record.Id, AssessmentId = record.AssessmentId,
            AssessmentTitle = assessment.Title, Score = record.Score,
            Passed = record.Passed, FinishedAt = record.FinishedAt
        });
    }

    [HttpGet("records")]
    public async Task<IActionResult> GetRecords()
    {
        var records = await _repo.GetRecordsByUserAsync(GetUserId());
        var dtos = records.Select(r => new AssessmentRecordDto
        {
            Id = r.Id, AssessmentId = r.AssessmentId,
            AssessmentTitle = r.Assessment.Title, Score = r.Score,
            Passed = r.Passed, FinishedAt = r.FinishedAt
        });
        return Ok(dtos);
    }
}
