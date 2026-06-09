using Microsoft.EntityFrameworkCore;
using SkillPlatform.Learning.Core.Entities;
using SkillPlatform.Learning.Core.Interfaces;
using SkillPlatform.Learning.Infra.Data;

namespace SkillPlatform.Learning.Infra.Repositories;

public class LearningRepository : ILearningRepository
{
    private readonly LearningDbContext _context;

    public LearningRepository(LearningDbContext context)
    {
        _context = context;
    }

    public async Task<LearningPath?> GetPathByUserAsync(Guid userId)
    {
        return await _context.LearningPaths.FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<LearningPath> UpsertPathAsync(LearningPath path)
    {
        var existing = await _context.LearningPaths.FirstOrDefaultAsync(p => p.UserId == path.UserId);
        if (existing != null)
        {
            existing.NodeIdsJson = path.NodeIdsJson;
            existing.ProgressJson = path.ProgressJson;
            existing.TargetDays = path.TargetDays;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            _context.LearningPaths.Add(path);
        }
        await _context.SaveChangesAsync();
        return existing ?? path;
    }

    public async Task<List<SkillAssessment>> GetAssessmentsByUserAsync(Guid userId)
    {
        return await _context.SkillAssessments
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.EvaluatedAt)
            .ToListAsync();
    }

    public async Task<SkillAssessment> UpsertAssessmentAsync(SkillAssessment assessment)
    {
        var existing = await _context.SkillAssessments
            .FirstOrDefaultAsync(a => a.UserId == assessment.UserId && a.NodeId == assessment.NodeId);
        if (existing != null)
        {
            existing.SelfScore = assessment.SelfScore;
            existing.Level = assessment.Level;
            existing.EvaluatedAt = DateTime.UtcNow;
        }
        else
        {
            _context.SkillAssessments.Add(assessment);
        }
        await _context.SaveChangesAsync();
        return existing ?? assessment;
    }

    public async Task<CheckIn> AddCheckInAsync(CheckIn checkIn)
    {
        var today = DateTime.UtcNow.Date;
        var existing = await _context.CheckIns
            .FirstOrDefaultAsync(c => c.UserId == checkIn.UserId && c.Date == today);
        if (existing != null)
        {
            existing.DurationMinutes += checkIn.DurationMinutes;
            existing.Note = checkIn.Note ?? existing.Note;
            await _context.SaveChangesAsync();
            return existing;
        }
        checkIn.Date = today;
        _context.CheckIns.Add(checkIn);
        await _context.SaveChangesAsync();
        return checkIn;
    }

    public async Task<List<CheckIn>> GetCheckInsByUserAsync(Guid userId, DateTime? from = null)
    {
        var query = _context.CheckIns.Where(c => c.UserId == userId);
        if (from.HasValue)
            query = query.Where(c => c.Date >= from.Value.Date);
        return await query.OrderByDescending(c => c.Date).Take(30).ToListAsync();
    }

    public async Task<int> GetStreakDaysAsync(Guid userId)
    {
        var dates = await _context.CheckIns
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.Date)
            .Select(c => c.Date)
            .ToListAsync();

        if (!dates.Any()) return 0;

        var streak = 1;
        var today = DateTime.UtcNow.Date;
        if (dates.First() != today) return 0;

        for (int i = 1; i < dates.Count; i++)
        {
            if (dates[i - 1].AddDays(-1) == dates[i])
                streak++;
            else
                break;
        }
        return streak;
    }

    public async Task<List<Assessment>> GetAssessmentsByNodeAsync(Guid nodeId)
    {
        return await _context.Assessments.Where(a => a.NodeId == nodeId).ToListAsync();
    }

    public async Task<Assessment?> GetAssessmentByIdAsync(Guid id)
    {
        return await _context.Assessments.FindAsync(id);
    }

    public async Task<AssessmentRecord> SubmitRecordAsync(AssessmentRecord record)
    {
        record.FinishedAt = DateTime.UtcNow;
        _context.AssessmentRecords.Add(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task<List<AssessmentRecord>> GetRecordsByUserAsync(Guid userId)
    {
        return await _context.AssessmentRecords
            .Where(r => r.UserId == userId)
            .Include(r => r.Assessment)
            .OrderByDescending(r => r.FinishedAt)
            .ToListAsync();
    }
}
