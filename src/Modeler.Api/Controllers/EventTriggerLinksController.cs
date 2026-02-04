using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/eventTriggerLinks")]
public sealed class EventTriggerLinksController : ControllerBase
{
    private readonly ModelerDbContext _db;
    public EventTriggerLinksController(ModelerDbContext db) => _db = db;

    [HttpGet]
    public async Task<List<EventTriggerLinkDto>> GetAll()
        => await _db.EventTriggerLinks.AsNoTracking()
            .OrderBy(x => x.EventId).ThenBy(x => x.TriggerId)
            .Select(x => x.ToDto())
            .ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventTriggerLinkDto>> GetById(int id)
    {
        var e = await _db.EventTriggerLinks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return e is null ? NotFound() : Ok(e.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<EventTriggerLinkDto>> Create([FromBody] EventTriggerLinkDto input)
    {
        // اگر خواستی می‌تونی اینجا چک کنی EventId/TriggerId وجود دارند
        var e = input.ToEntity();
        e.Id = 0;
        _db.EventTriggerLinks.Add(e);
        await _db.SaveChangesAsync();
        return Ok(e.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EventTriggerLinkDto input)
    {
        var e = await _db.EventTriggerLinks.FirstOrDefaultAsync(x => x.Id == id);
        if (e is null) return NotFound();

        e.EventId = input.EventId;
        e.TriggerId = input.TriggerId;

        await _db.SaveChangesAsync();
        return Ok(e.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.EventTriggerLinks.FirstOrDefaultAsync(x => x.Id == id);
        if (e is null) return NotFound();
        _db.EventTriggerLinks.Remove(e);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
