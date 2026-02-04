using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/triggers")]
public sealed class TriggersController : ControllerBase
{
    private readonly ModelerDbContext _db;
    public TriggersController(ModelerDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<TriggerDefinition>>> GetAll()
        => await _db.Triggers.AsNoTracking().OrderBy(x => x.TriggerKey).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TriggerDefinition>> GetById(int id)
    {
        var row = await _db.Triggers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return row == null ? NotFound() : row;
    }

    [HttpPost]
    public async Task<ActionResult<TriggerDefinition>> Create([FromBody] TriggerDefinition input)
    {
        input.Id = 0;
        _db.Triggers.Add(input);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TriggerDefinition>> Update(int id, [FromBody] TriggerDefinition input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var row = await _db.Triggers.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();

        row.TriggerKey = input.TriggerKey;
        row.TitleFa = input.TitleFa;
        row.Description = input.Description;

        await _db.SaveChangesAsync();
        return row;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.Triggers.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();

        _db.Triggers.Remove(row);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
