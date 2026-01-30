using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/scenario-decision-options")]
public sealed class ScenarioDecisionOptionController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public ScenarioDecisionOptionController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ScenarioDecisionOptionDto>>> GetAll([FromQuery] int? scenarioDecisionId)
    {
        IQueryable<ScenarioDecisionOption> q = _db.ScenarioDecisionOptions.AsNoTracking();
        
        if (scenarioDecisionId.HasValue) q = q.Where(x => x.ScenarioDecisionId == scenarioDecisionId.Value);

        var rows = await q.OrderBy(x => x.OptionKey).ToListAsync();
        return rows.Select(Map.ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScenarioDecisionOptionDto>> GetById(int id)
    {
        var row = await _db.ScenarioDecisionOptions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();
        return Map.ToDto(row);
    }

    [HttpPost]
    public async Task<ActionResult<ScenarioDecisionOptionDto>> Create(ScenarioDecisionOptionDto input)
    {
        // برای Create، Id رو نادیده می‌گیریم تا Identity تولید کنه
        input.Id = 0;

        var entity = Map.ToEntity(input);
        _db.ScenarioDecisionOptions.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, Map.ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ScenarioDecisionOptionDto>> Update(int id, ScenarioDecisionOptionDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var exists = await CrudHelpers.ExistsAsync<ScenarioDecisionOption>(_db, id);
        if (!exists) return NotFound();

        var entity = Map.ToEntity(input);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return Map.ToDto(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await CrudHelpers.FindAsync<ScenarioDecisionOption>(_db, id);
        if (row == null) return NotFound();

        _db.ScenarioDecisionOptions.Remove(row);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
