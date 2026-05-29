using System.Text.Json;
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

        var validationError = await ValidateInputAsync(input);
        if (validationError != null) return validationError;

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

        var validationError = await ValidateInputAsync(input);
        if (validationError != null) return validationError;

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

    private async Task<ActionResult<KartablRoutingRuleDto>?> ValidateInputAsync(KartablRoutingRuleDto input)
    {
        if (string.IsNullOrWhiteSpace(input.RuleKey))
            return BadRequest("RuleKey is required.");

        if (input.TargetKartablId <= 0)
            return BadRequest("TargetKartablId is required.");

        var targetExists = await _db.Kartabls.AsNoTracking().AnyAsync(k => k.Id == input.TargetKartablId);
        if (!targetExists)
            return BadRequest($"TargetKartablId not found: {input.TargetKartablId}");

        if (input.FromKartablId.HasValue)
        {
            var fromExists = await _db.Kartabls.AsNoTracking().AnyAsync(k => k.Id == input.FromKartablId.Value);
            if (!fromExists)
                return BadRequest($"FromKartablId not found: {input.FromKartablId.Value}");
        }

        List<int>? conditionIds;
        try
        {
            conditionIds = JsonSerializer.Deserialize<List<int>>(input.ConditionIdsJson);
        }
        catch (Exception ex)
        {
            return BadRequest($"ConditionIdsJson must be a JSON array of integers. {ex.Message}");
        }

        conditionIds ??= new List<int>();
        conditionIds = conditionIds.Where(id => id > 0).Distinct().ToList();
        input.ConditionIdsJson = JsonSerializer.Serialize(conditionIds);

        if (conditionIds.Count > 0)
        {
            var existingIds = await _db.Conditions.AsNoTracking()
                .Where(c => conditionIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            var missing = conditionIds.Except(existingIds).ToList();
            if (missing.Count > 0)
                return BadRequest($"ConditionIdsJson references missing condition(s): [{string.Join(",", missing)}].");
        }

        return null;
    }
}
