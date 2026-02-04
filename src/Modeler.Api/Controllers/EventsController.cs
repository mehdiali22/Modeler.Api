using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/events")]
public sealed class EventsController : ControllerBase
{
    private readonly ModelerDbContext _db;
    public EventsController(ModelerDbContext db) => _db = db;

    [HttpGet]
    public async Task<List<EventDefinitionDto>> GetAll()
        => await _db.Events.AsNoTracking()
            .OrderBy(x => x.EventKey)
            .Select(x => x.ToDto())
            .ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventDefinitionDto>> GetById(int id)
    {
        var e = await _db.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return e is null ? NotFound() : Ok(e.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<EventDefinitionDto>> Create([FromBody] EventDefinitionDto input)
    {
        var e = input.ToEntity();
        e.Id = 0;
        _db.Events.Add(e);
        await _db.SaveChangesAsync();
        return Ok(e.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EventDefinitionDto input)
    {
        var e = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
        if (e is null) return NotFound();

        e.EventKey = input.EventKey;
        e.TitleFa = input.TitleFa;
        e.Description = input.Description;

        await _db.SaveChangesAsync();
        return Ok(e.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
        if (e is null) return NotFound();
        _db.Events.Remove(e);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
