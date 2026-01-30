using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/scenario-preconditions")]
public sealed class ScenarioPreconditionController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public ScenarioPreconditionController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ScenarioPreconditionDto>>> GetAll([FromQuery] int? scenarioId, [FromQuery] int? conditionId)
    {
        IQueryable<ScenarioPrecondition> q = _db.ScenarioPreconditions.AsNoTracking();

        if (scenarioId.HasValue) q = q.Where(x => x.ScenarioId == scenarioId.Value);
        if (conditionId.HasValue) q = q.Where(x => x.ConditionId == conditionId.Value);

        var rows = await q.ToListAsync();
        return rows.Select(x => new ScenarioPreconditionDto { ScenarioId = x.ScenarioId, ConditionId = x.ConditionId }).ToList();
    }

    [HttpPost]
    public async Task<ActionResult> Create(ScenarioPreconditionDto input)
    {
        var exists = await _db.ScenarioPreconditions.AnyAsync(x =>
            x.ScenarioId == input.ScenarioId && x.ConditionId == input.ConditionId);

        if (exists) return Conflict("link already exists");

        _db.ScenarioPreconditions.Add(new ScenarioPrecondition { ScenarioId = input.ScenarioId, ConditionId = input.ConditionId });
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int scenarioId, [FromQuery] int conditionId)
    {
        var row = await _db.ScenarioPreconditions.FirstOrDefaultAsync(x =>
            x.ScenarioId == scenarioId && x.ConditionId == conditionId);

        if (row == null) return NotFound();

        _db.ScenarioPreconditions.Remove(row);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
