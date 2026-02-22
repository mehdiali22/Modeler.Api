using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/kartabl-routing-rules")]
public sealed class KartablRoutingRuleController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public KartablRoutingRuleController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<KartablRoutingRuleDto>>> GetAll()
    {
        var rows = await _db.KartablRoutingRules
            .AsNoTracking()
            .OrderBy(x => x.Priority)
            .ThenBy(x => x.RuleKey)
            .ToListAsync();

        return rows.Select(Map.ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<KartablRoutingRuleDto>> GetById(int id)
    {
        var row = await _db.KartablRoutingRules.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();
        return Map.ToDto(row);
    }

    [HttpPost]
    public async Task<ActionResult<KartablRoutingRuleDto>> Create([FromBody] KartablRoutingRuleDto input)
    {
        input.Id = 0;
        input.ConditionIdsJson = string.IsNullOrWhiteSpace(input.ConditionIdsJson) ? "[]" : input.ConditionIdsJson;

        var entity = Map.ToEntity(input);
        _db.KartablRoutingRules.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, Map.ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<KartablRoutingRuleDto>> Update(int id, [FromBody] KartablRoutingRuleDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");
        input.ConditionIdsJson = string.IsNullOrWhiteSpace(input.ConditionIdsJson) ? "[]" : input.ConditionIdsJson;

        var exists = await CrudHelpers.ExistsAsync<KartablRoutingRule>(_db, id);
        if (!exists) return NotFound();

        var entity = Map.ToEntity(input);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return Map.ToDto(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await CrudHelpers.FindAsync<KartablRoutingRule>(_db, id);
        if (row == null) return NotFound();

        _db.KartablRoutingRules.Remove(row);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
