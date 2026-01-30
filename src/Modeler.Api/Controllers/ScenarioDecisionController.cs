using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/scenario-decisions")]
public sealed class ScenarioDecisionController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public ScenarioDecisionController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ScenarioDecisionDto>>> GetAll([FromQuery] int? scenarioId)
    {
        IQueryable<ScenarioDecision> q = _db.ScenarioDecisions.AsNoTracking();
        
        if (scenarioId.HasValue) q = q.Where(x => x.ScenarioId == scenarioId.Value);

        var rows = await q.OrderBy(x => x.DecisionKey).ToListAsync();
        return rows.Select(Map.ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScenarioDecisionDto>> GetById(int id)
    {
        var row = await _db.ScenarioDecisions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();
        return Map.ToDto(row);
    }

    [HttpPost]
    public async Task<ActionResult<ScenarioDecisionDto>> Create(ScenarioDecisionDto input)
    {
        // برای Create، Id رو نادیده می‌گیریم تا Identity تولید کنه
        input.Id = 0;

        var entity = Map.ToEntity(input);
        _db.ScenarioDecisions.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, Map.ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ScenarioDecisionDto>> Update(int id, ScenarioDecisionDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var exists = await CrudHelpers.ExistsAsync<ScenarioDecision>(_db, id);
        if (!exists) return NotFound();

        var entity = Map.ToEntity(input);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return Map.ToDto(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await CrudHelpers.FindAsync<ScenarioDecision>(_db, id);
        if (row == null) return NotFound();

        _db.ScenarioDecisions.Remove(row);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
