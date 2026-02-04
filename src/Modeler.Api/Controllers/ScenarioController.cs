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
        var scenarios = await _db.Scenarios
            .AsNoTracking()
            .OrderBy(x => x.ScenarioKey)
            .ToListAsync();

        var scenarioIds = scenarios.Select(x => x.Id).ToList();

        var pres = await _db.ScenarioPreconditions
            .AsNoTracking()
            .Where(x => scenarioIds.Contains(x.ScenarioId))
            .ToListAsync();

        var fcs = await _db.ScenarioFactChanges
            .AsNoTracking()
            .Where(x => scenarioIds.Contains(x.ScenarioId))
            .ToListAsync();

        // group for fast lookup
        var preByScenario = pres
            .GroupBy(x => x.ScenarioId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.ConditionId).ToList());

        var fcByScenario = fcs
            .GroupBy(x => x.ScenarioId)
            .ToDictionary(g => g.Key, g => g.Select(ToDto).ToList());

        var result = new List<ScenarioDto>(scenarios.Count);

        foreach (var s in scenarios)
        {
            var dto = Map.ToDto(s);
            dto.TriggerId = s.TriggerId;

            dto.PreconditionIds = preByScenario.TryGetValue(s.Id, out var preList)
                ? preList
                : new List<int>();

            dto.FactChanges = fcByScenario.TryGetValue(s.Id, out var fcList)
                ? fcList
                : new List<ScenarioFactChangeDto>();

            result.Add(dto);
        }

        return result;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScenarioDto>> GetById(int id)
    {
        var s = await _db.Scenarios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (s == null) return NotFound();

        var pres = await _db.ScenarioPreconditions.AsNoTracking()
            .Where(x => x.ScenarioId == id)
            .ToListAsync();

        var fcs = await _db.ScenarioFactChanges.AsNoTracking()
            .Where(x => x.ScenarioId == id)
            .ToListAsync();

        var dto = Map.ToDto(s);
        dto.TriggerId = s.TriggerId;
        dto.PreconditionIds = pres.Select(x => x.ConditionId).ToList();
        dto.FactChanges = fcs.Select(ToDto).ToList();

        return dto;
    }

    [HttpPost]
    public async Task<ActionResult<ScenarioDto>> Create([FromBody] ScenarioDto input)
    {
        input.Id = 0;

        var entity = Map.ToEntity(input);
        entity.Id = 0;

        // NEW
        entity.TriggerId = input.TriggerId;

        _db.Scenarios.Add(entity);
        await _db.SaveChangesAsync();

        await SyncPreconditions(entity.Id, input.PreconditionIds);
        await SyncFactChanges(entity.Id, input.FactChanges);

        // return full dto
        var saved = await _db.Scenarios.AsNoTracking().FirstAsync(x => x.Id == entity.Id);
        var dto = Map.ToDto(saved);
        dto.TriggerId = saved.TriggerId;
        dto.PreconditionIds = (input.PreconditionIds ?? new()).ToList();
        dto.FactChanges = (input.FactChanges ?? new()).Select(x =>
        {
            x.Id = 0;              // client ids ignored
            x.ScenarioId = saved.Id;
            return x;
        }).ToList();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ScenarioDto>> Update(int id, [FromBody] ScenarioDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var row = await _db.Scenarios.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();

        // update base fields
        row.ScenarioKey = input.ScenarioKey;
        row.TitleFa = input.TitleFa;
        row.Description = input.Description;
        row.StageId = input.StageId;
        row.OwnerSubdomain = input.OwnerSubdomain;

        // NEW
        row.TriggerId = input.TriggerId;

        await _db.SaveChangesAsync();

        await SyncPreconditions(id, input.PreconditionIds);
        await SyncFactChanges(id, input.FactChanges);

        // return full dto (fresh)
        var dto = Map.ToDto(row);
        dto.TriggerId = row.TriggerId;
        dto.PreconditionIds = (input.PreconditionIds ?? new()).ToList();
        dto.FactChanges = (input.FactChanges ?? new()).Select(x =>
        {
            x.ScenarioId = id;
            return x;
        }).ToList();

        return dto;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        // delete children first
        var pres = await _db.ScenarioPreconditions.Where(x => x.ScenarioId == id).ToListAsync();
        _db.ScenarioPreconditions.RemoveRange(pres);

        var fcs = await _db.ScenarioFactChanges.Where(x => x.ScenarioId == id).ToListAsync();
        _db.ScenarioFactChanges.RemoveRange(fcs);

        var row = await _db.Scenarios.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();

        _db.Scenarios.Remove(row);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private async Task SyncPreconditions(int scenarioId, List<int>? conditionIds)
    {
        conditionIds ??= new List<int>();

        var existing = await _db.ScenarioPreconditions
            .Where(x => x.ScenarioId == scenarioId)
            .ToListAsync();

        _db.ScenarioPreconditions.RemoveRange(existing);

        if (conditionIds.Count > 0)
        {
            var rows = conditionIds.Distinct()
                .Select(cid => new ScenarioPrecondition
                {
                    ScenarioId = scenarioId,
                    ConditionId = cid
                })
                .ToList();

            _db.ScenarioPreconditions.AddRange(rows);
        }

        await _db.SaveChangesAsync();
    }

    private async Task SyncFactChanges(int scenarioId, List<ScenarioFactChangeDto>? factChanges)
    {
        factChanges ??= new List<ScenarioFactChangeDto>();

        var existing = await _db.ScenarioFactChanges
            .Where(x => x.ScenarioId == scenarioId)
            .ToListAsync();

        _db.ScenarioFactChanges.RemoveRange(existing);

        if (factChanges.Count > 0)
        {
            var rows = factChanges.Select(fc => new ScenarioFactChange
            {
                Id = 0,
                ScenarioId = scenarioId,
                FactId = fc.FactId,
                Op = fc.Op,
                Value = fc.Value
            }).ToList();

            _db.ScenarioFactChanges.AddRange(rows);
        }

        await _db.SaveChangesAsync();
    }

    private static ScenarioFactChangeDto ToDto(ScenarioFactChange e) => new()
    {
        Id = e.Id,
        ScenarioId = e.ScenarioId,
        FactId = e.FactId,
        Op = e.Op,
        Value = e.Value
    };
}
