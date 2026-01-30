using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/scenarios")]
public sealed class ScenarioController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public ScenarioController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ScenarioDto>>> GetAll()
    {
        IQueryable<Scenario> q = _db.Scenarios.AsNoTracking();
        

        var rows = await q.OrderBy(x => x.ScenarioKey).ToListAsync();
        return rows.Select(Map.ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScenarioDto>> GetById(int id)
    {
        var row = await _db.Scenarios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();
        return Map.ToDto(row);
    }

    [HttpPost]
    public async Task<ActionResult<ScenarioDto>> Create(ScenarioDto input)
    {
        // برای Create، Id رو نادیده می‌گیریم تا Identity تولید کنه
        input.Id = 0;

        var entity = Map.ToEntity(input);
        _db.Scenarios.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, Map.ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ScenarioDto>> Update(int id, ScenarioDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var exists = await CrudHelpers.ExistsAsync<Scenario>(_db, id);
        if (!exists) return NotFound();

        var entity = Map.ToEntity(input);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return Map.ToDto(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await CrudHelpers.FindAsync<Scenario>(_db, id);
        if (row == null) return NotFound();

        _db.Scenarios.Remove(row);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
