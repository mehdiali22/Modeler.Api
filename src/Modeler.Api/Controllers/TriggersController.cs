using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/triggers")]
public sealed class TriggersController : ControllerBase
{
    private readonly ModelerDbContext _db;
    public TriggersController(ModelerDbContext db) => _db = db;

    [HttpGet]
    public async Task<List<TriggerDefinitionDto>> GetAll()
        => await _db.Triggers.AsNoTracking()
            .OrderBy(x => x.TriggerKey)
            .Select(x => x.ToDto())
            .ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TriggerDefinitionDto>> GetById(int id)
    {
        var e = await _db.Triggers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return e is null ? NotFound() : Ok(e.ToDto());
    }

    [HttpPost]
    public async Task<ActionResult<TriggerDefinitionDto>> Create([FromBody] TriggerDefinitionDto input)
    {
        var e = input.ToEntity();
        e.Id = 0;
        _db.Triggers.Add(e);
        await _db.SaveChangesAsync();
        return Ok(e.ToDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TriggerDefinitionDto input)
    {
        var e = await _db.Triggers.FirstOrDefaultAsync(x => x.Id == id);
        if (e is null) return NotFound();

        e.TriggerKey = input.TriggerKey;
        e.TitleFa = input.TitleFa;
        e.Description = input.Description;

        await _db.SaveChangesAsync();
        return Ok(e.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Triggers.FirstOrDefaultAsync(x => x.Id == id);
        if (e is null) return NotFound();
        _db.Triggers.Remove(e);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
