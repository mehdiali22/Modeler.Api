using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/action-state-transitions")]
public sealed class ActionStateTransitionController : ControllerBase
{
    private readonly ModelerDbContext _db;
    public ActionStateTransitionController(ModelerDbContext db) => _db = db;

    [HttpGet]
    public async Task<List<ActionStateTransition>> Get([FromQuery] int? scenarioId, [FromQuery] int? actionId) =>
        await _db.ActionStateTransitions.AsNoTracking()
            .Where(x => scenarioId == null || x.ScenarioId == scenarioId)
            .Where(x => actionId == null || x.ActionId == actionId)
            .OrderBy(x => x.SortOrder).ThenBy(x => x.Id)
            .ToListAsync();

    [HttpPost]
    public async Task<ActionResult<ActionStateTransition>> Create(ActionStateTransition row)
    {
        row.Id = 0;
        _db.ActionStateTransitions.Add(row);
        await _db.SaveChangesAsync();
        return row;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ActionStateTransition>> Update(int id, ActionStateTransition row)
    {
        if (id != row.Id) return BadRequest("id mismatch");
        _db.Entry(row).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return row;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.ActionStateTransitions.FindAsync(id);
        if (row == null) return NotFound();
        _db.ActionStateTransitions.Remove(row);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
