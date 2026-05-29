using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using EntityStateModel = Modeler.Api.Domain.EntityStatee;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/entity-states")]
public sealed class EntityStateController : ControllerBase
{
    private readonly ModelerDbContext _db;
    public EntityStateController(ModelerDbContext db) => _db = db;

    [HttpGet]
    public async Task<List<EntityStateModel>> Get([FromQuery] int? artifactId) =>
        await _db.EntityStates.AsNoTracking()
            .Where(x => artifactId == null || x.ArtifactId == artifactId)
            .OrderBy(x => x.ArtifactId).ThenBy(x => x.StateKey)
            .ToListAsync();

    [HttpPost]
    public async Task<ActionResult<EntityStateModel>> Create(EntityStateModel row)
    {
        row.Id = 0;
        row.ConditionJson = string.IsNullOrWhiteSpace(row.ConditionJson) ? "[]" : row.ConditionJson;
        _db.EntityStates.Add(row);
        await _db.SaveChangesAsync();
        return row;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EntityStateModel>> Update(int id, EntityStateModel row)
    {
        if (id != row.Id) return BadRequest("id mismatch");
        row.ConditionJson = string.IsNullOrWhiteSpace(row.ConditionJson) ? "[]" : row.ConditionJson;
        _db.Entry(row).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        await _db.SaveChangesAsync();
        return row;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.EntityStates.FindAsync(id);
        if (row == null) return NotFound();
        _db.EntityStates.Remove(row);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
