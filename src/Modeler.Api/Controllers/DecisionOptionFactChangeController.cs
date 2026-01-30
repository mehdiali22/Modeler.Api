using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/decision-option-fact-changes")]
public sealed class DecisionOptionFactChangeController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public DecisionOptionFactChangeController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<DecisionOptionFactChangeDto>>> GetAll([FromQuery] int? scenarioDecisionOptionId)
    {
        IQueryable<DecisionOptionFactChange> q = _db.DecisionOptionFactChanges.AsNoTracking();
        
        if (scenarioDecisionOptionId.HasValue) q = q.Where(x => x.ScenarioDecisionOptionId == scenarioDecisionOptionId.Value);

        var rows = await q.OrderBy(x => x.Id).ToListAsync();
        return rows.Select(Map.ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DecisionOptionFactChangeDto>> GetById(int id)
    {
        var row = await _db.DecisionOptionFactChanges.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();
        return Map.ToDto(row);
    }

    [HttpPost]
    public async Task<ActionResult<DecisionOptionFactChangeDto>> Create(DecisionOptionFactChangeDto input)
    {
        // برای Create، Id رو نادیده می‌گیریم تا Identity تولید کنه
        input.Id = 0;

        var entity = Map.ToEntity(input);
        _db.DecisionOptionFactChanges.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, Map.ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<DecisionOptionFactChangeDto>> Update(int id, DecisionOptionFactChangeDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var exists = await CrudHelpers.ExistsAsync<DecisionOptionFactChange>(_db, id);
        if (!exists) return NotFound();

        var entity = Map.ToEntity(input);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return Map.ToDto(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await CrudHelpers.FindAsync<DecisionOptionFactChange>(_db, id);
        if (row == null) return NotFound();

        _db.DecisionOptionFactChanges.Remove(row);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
