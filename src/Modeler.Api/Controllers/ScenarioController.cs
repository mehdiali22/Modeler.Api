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

        var pes = await _db.ScenarioProducedEvents
            .AsNoTracking()
            .Where(x => scenarioIds.Contains(x.ScenarioId))
            .ToListAsync();

        var acts = await _db.ScenarioActions
            .AsNoTracking()
            .Where(x => scenarioIds.Contains(x.ScenarioId))
            .ToListAsync();

        var preByScenario = pres
            .GroupBy(x => x.ScenarioId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.ConditionId).ToList());

        var fcByScenario = fcs
            .GroupBy(x => x.ScenarioId)
            .ToDictionary(g => g.Key, g => g.Select(ToScenarioFactChangeDto).ToList());

        var peByScenario = pes
            .GroupBy(x => x.ScenarioId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.EventId).Distinct().ToList());

        var actByScenario = acts
            .GroupBy(x => x.ScenarioId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => new ScenarioActionRefDto
                {
                    ActionId = x.ActionId,
                    ParamsJson = x.ParamsJson
                }).ToList()
            );

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

            dto.ProducedEventIds = peByScenario.TryGetValue(s.Id, out var peList)
                ? peList
                : new List<int>();

            dto.Actions = actByScenario.TryGetValue(s.Id, out var aList)
                ? aList
                : new List<ScenarioActionRefDto>();

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

        var pes = await _db.ScenarioProducedEvents.AsNoTracking()
            .Where(x => x.ScenarioId == id)
            .ToListAsync();

        var acts = await _db.ScenarioActions.AsNoTracking()
            .Where(x => x.ScenarioId == id)
            .ToListAsync();

        var dto = Map.ToDto(s);
        dto.TriggerId = s.TriggerId;
        dto.PreconditionIds = pres.Select(x => x.ConditionId).ToList();
        dto.FactChanges = fcs.Select(ToScenarioFactChangeDto).ToList();
        dto.ProducedEventIds = pes.Select(x => x.EventId).Distinct().ToList();
        dto.Actions = acts.Select(x => new ScenarioActionRefDto
        {
            ActionId = x.ActionId,
            ParamsJson = x.ParamsJson
        }).ToList();

        return dto;
    }

    [HttpPost]
    public async Task<ActionResult<ScenarioDto>> Create([FromBody] ScenarioDto input)
    {
        input.Id = 0;

        var entity = Map.ToEntity(input);
        entity.Id = 0;
        entity.TriggerId = input.TriggerId;

        _db.Scenarios.Add(entity);
        await _db.SaveChangesAsync();

        await SyncPreconditions(entity.Id, input.PreconditionIds);
        await SyncFactChanges(entity.Id, input.FactChanges);
        await SyncProducedEvents(entity.Id, input.ProducedEventIds);
        await SyncActions(entity.Id, input.Actions);

        // response (full-ish)
        var saved = await _db.Scenarios.AsNoTracking().FirstAsync(x => x.Id == entity.Id);
        var dto = Map.ToDto(saved);
        dto.TriggerId = saved.TriggerId;

        dto.PreconditionIds = (input.PreconditionIds ?? new()).ToList();

        dto.FactChanges = (input.FactChanges ?? new()).Select(x =>
        {
            x.Id = 0;
            x.ScenarioId = saved.Id;
            return x;
        }).ToList();

        dto.ProducedEventIds = (input.ProducedEventIds ?? new()).Distinct().ToList();
        dto.Actions = (input.Actions ?? new()).ToList();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ScenarioDto>> Update(int id, [FromBody] ScenarioDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var row = await _db.Scenarios.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();

        row.ScenarioKey = input.ScenarioKey;
        row.TitleFa = input.TitleFa;
        row.Description = input.Description;
        row.StageId = input.StageId;
        row.OwnerSubdomain = input.OwnerSubdomain;
        row.TriggerId = input.TriggerId;

        await _db.SaveChangesAsync();

        await SyncPreconditions(id, input.PreconditionIds);
        await SyncFactChanges(id, input.FactChanges);
        await SyncProducedEvents(id, input.ProducedEventIds);
        await SyncActions(id, input.Actions);

        var dto = Map.ToDto(row);
        dto.TriggerId = row.TriggerId;
        dto.PreconditionIds = (input.PreconditionIds ?? new()).ToList();
        dto.FactChanges = (input.FactChanges ?? new()).Select(x => { x.ScenarioId = id; return x; }).ToList();
        dto.ProducedEventIds = (input.ProducedEventIds ?? new()).Distinct().ToList();
        dto.Actions = (input.Actions ?? new()).ToList();

        return dto;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        // children first
        var scActs = await _db.ScenarioActions.Where(x => x.ScenarioId == id).ToListAsync();
        _db.ScenarioActions.RemoveRange(scActs);

        var scPes = await _db.ScenarioProducedEvents.Where(x => x.ScenarioId == id).ToListAsync();
        _db.ScenarioProducedEvents.RemoveRange(scPes);

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

    // -----------------------
    // Sync helpers
    // -----------------------

    private async Task SyncPreconditions(int scenarioId, List<int>? conditionIds)
    {
        conditionIds ??= new();

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
        factChanges ??= new();

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

    private async Task SyncProducedEvents(int scenarioId, List<int>? eventIds)
    {
        eventIds ??= new();

        var existing = await _db.ScenarioProducedEvents
            .Where(x => x.ScenarioId == scenarioId)
            .ToListAsync();

        _db.ScenarioProducedEvents.RemoveRange(existing);

        if (eventIds.Count > 0)
        {
            var rows = eventIds.Distinct()
                .Select(eid => new ScenarioProducedEvent
                {
                    ScenarioId = scenarioId,
                    EventId = eid
                })
                .ToList();

            _db.ScenarioProducedEvents.AddRange(rows);
        }

        await _db.SaveChangesAsync();
    }

    private async Task SyncActions(int scenarioId, List<ScenarioActionRefDto>? actions)
    {
        actions ??= new();

        var existing = await _db.ScenarioActions
            .Where(x => x.ScenarioId == scenarioId)
            .ToListAsync();

        _db.ScenarioActions.RemoveRange(existing);

        if (actions.Count > 0)
        {
            // de-dupe by ActionId (first wins)
            var rows = actions
                .Where(x => x.ActionId > 0)
                .GroupBy(x => x.ActionId)
                .Select(g => g.First())
                .Select(a => new ScenarioAction
                {
                    ScenarioId = scenarioId,
                    ActionId = a.ActionId,
                    ParamsJson = a.ParamsJson
                })
                .ToList();

            _db.ScenarioActions.AddRange(rows);
        }

        await _db.SaveChangesAsync();
    }

    private static ScenarioFactChangeDto ToScenarioFactChangeDto(ScenarioFactChange e) => new()
    {
        Id = e.Id,
        ScenarioId = e.ScenarioId,
        FactId = e.FactId,
        Op = e.Op,
        Value = e.Value
    };
}
