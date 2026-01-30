using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/condition-fact-used")]
public sealed class ConditionFactUsedController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public ConditionFactUsedController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ConditionFactUsedDto>>> GetAll([FromQuery] int? conditionId, [FromQuery] int? factId)
    {
        IQueryable<ConditionFactUsed> q = _db.ConditionFactUsed.AsNoTracking();

        if (conditionId.HasValue) q = q.Where(x => x.ConditionId == conditionId.Value);
        if (factId.HasValue) q = q.Where(x => x.FactId == factId.Value);

        var rows = await q.ToListAsync();
        return rows.Select(x => new ConditionFactUsedDto { ConditionId = x.ConditionId, FactId = x.FactId }).ToList();
    }

    [HttpPost]
    public async Task<ActionResult> Create(ConditionFactUsedDto input)
    {
        var exists = await _db.ConditionFactUsed.AnyAsync(x =>
            x.ConditionId == input.ConditionId && x.FactId == input.FactId);

        if (exists) return Conflict("link already exists");

        _db.ConditionFactUsed.Add(new ConditionFactUsed { ConditionId = input.ConditionId, FactId = input.FactId });
        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int conditionId, [FromQuery] int factId)
    {
        var row = await _db.ConditionFactUsed.FirstOrDefaultAsync(x =>
            x.ConditionId == conditionId && x.FactId == factId);

        if (row == null) return NotFound();

        _db.ConditionFactUsed.Remove(row);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
