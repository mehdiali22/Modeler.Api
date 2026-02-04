using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/events")]
public sealed class EventsController : ControllerBase
{
    private readonly ModelerDbContext _db;
    public EventsController(ModelerDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<EventDefinition>>> GetAll()
        => await _db.Events.AsNoTracking().OrderBy(x => x.EventKey).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventDefinition>> GetById(int id)
    {
        var row = await _db.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return row == null ? NotFound() : row;
    }

    [HttpPost]
    public async Task<ActionResult<EventDefinition>> Create([FromBody] EventDefinition input)
    {
        input.Id = 0;
        _db.Events.Add(input);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EventDefinition>> Update(int id, [FromBody] EventDefinition input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var row = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();

        row.EventKey = input.EventKey;
        row.TitleFa = input.TitleFa;
        row.Description = input.Description;

        await _db.SaveChangesAsync();
        return row;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();

        _db.Events.Remove(row);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
