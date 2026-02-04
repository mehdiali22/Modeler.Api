using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/eventTriggerLinks")]
public sealed class EventTriggerLinksController : ControllerBase
{
    private readonly ModelerDbContext _db;
    public EventTriggerLinksController(ModelerDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<EventTriggerLink>>> GetAll()
        => await _db.EventTriggerLinks.AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventTriggerLink>> GetById(int id)
    {
        var row = await _db.EventTriggerLinks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return row == null ? NotFound() : row;
    }

    [HttpPost]
    public async Task<ActionResult<EventTriggerLink>> Create([FromBody] EventTriggerLink input)
    {
        input.Id = 0;
        _db.EventTriggerLinks.Add(input);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EventTriggerLink>> Update(int id, [FromBody] EventTriggerLink input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var row = await _db.EventTriggerLinks.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();

        row.EventId = input.EventId;
        row.TriggerId = input.TriggerId;

        await _db.SaveChangesAsync();
        return row;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.EventTriggerLinks.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();

        _db.EventTriggerLinks.Remove(row);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
