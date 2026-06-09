using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillPlatform.Collaboration.Core.DTOs;
using SkillPlatform.Collaboration.Core.Interfaces;

namespace SkillPlatform.Collaboration.Api.Controllers;

[ApiController]
[Route("api/notes")]
[Authorize]
public class NotesController : ControllerBase
{
    private readonly ICollaborationRepository _repo;
    public NotesController(ICollaborationRepository repo) => _repo = repo;

    private Guid GetUserId()
    {
        var c = User.FindFirst(ClaimTypes.NameIdentifier);
        return c != null ? Guid.Parse(c.Value) : Guid.Empty;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotes([FromQuery] Guid node)
    {
        var notes = await _repo.GetNotesByNodeAsync(node);
        var dtos = notes.Select(n => new NoteDto { Id = n.Id, Title = n.Title, Content = n.Content, NodeId = n.NodeId, UserId = n.UserId, Status = n.Status, ViewCount = n.ViewCount, LikeCount = n.LikeCount, CreatedAt = n.CreatedAt });
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetNote(Guid id)
    {
        var n = await _repo.GetNoteByIdAsync(id);
        if (n == null) return NotFound();
        n.ViewCount++;
        await _repo.UpdateNoteAsync(n);
        return Ok(new NoteDto { Id = n.Id, Title = n.Title, Content = n.Content, NodeId = n.NodeId, UserId = n.UserId, Status = n.Status, ViewCount = n.ViewCount, LikeCount = n.LikeCount, CreatedAt = n.CreatedAt });
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest req)
    {
        var note = await _repo.CreateNoteAsync(new Core.Entities.Note
        {
            Id = Guid.NewGuid(), Title = req.Title, Content = req.Content,
            NodeId = req.NodeId, UserId = GetUserId()
        });
        return CreatedAtAction(nameof(GetNote), new { id = note.Id }, note);
    }
}
