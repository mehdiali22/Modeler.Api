using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;
using Modeler.Api.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/work-items")]
public sealed class WorkItemsController : ControllerBase
{
    private readonly ModelerDbContext _db;
    private readonly KartablRoutingEngine _routing;

    public WorkItemsController(ModelerDbContext db, KartablRoutingEngine routing)
    {
        _db = db;
        _routing = routing;
    }

    [HttpGet]
    public async Task<ActionResult<List<WorkItemDto>>> GetAll()
    {
        var rows = await _db.WorkItems
            .AsNoTracking()
            .OrderByDescending(x => x.UpdatedAtUtc)
            .ThenBy(x => x.Id)
            .ToListAsync();

        return rows.Select(Map.ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkItemDto>> GetById(int id)
    {
        var row = await _db.WorkItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();
        return Map.ToDto(row);
    }

    [HttpPost]
    public async Task<ActionResult<WorkItemDto>> Create([FromBody] WorkItemDto input)
    {
        input.Id = 0;
        var entity = Map.ToEntity(input);

        // basic FK validation (optional)
        if (entity.CurrentKartablId.HasValue)
        {
            var ok = await _db.Kartabls.AsNoTracking().AnyAsync(k => k.Id == entity.CurrentKartablId.Value);
            if (!ok) return BadRequest($"Kartabl not found: {entity.CurrentKartablId.Value}");
        }

        _db.WorkItems.Add(entity);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, Map.ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<WorkItemDto>> Update(int id, [FromBody] WorkItemDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var exists = await _db.WorkItems.AsNoTracking().AnyAsync(x => x.Id == id);
        if (!exists) return NotFound();

        if (input.CurrentKartablId.HasValue)
        {
            var ok = await _db.Kartabls.AsNoTracking().AnyAsync(k => k.Id == input.CurrentKartablId.Value);
            if (!ok) return BadRequest($"Kartabl not found: {input.CurrentKartablId.Value}");
        }

        var entity = Map.ToEntity(input);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return Map.ToDto(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.WorkItems.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();

        _db.WorkItems.Remove(row);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id:int}/actions")]
    public async Task<ActionResult<List<WorkItemActionDto>>> GetActions(int id, [FromQuery] string? status = null)
    {
        var exists = await _db.WorkItems.AsNoTracking().AnyAsync(x => x.Id == id);
        if (!exists) return NotFound();

        var query = _db.WorkItemActions
            .AsNoTracking()
            .Where(x => x.WorkItemId == id);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(x => x.Status == status.Trim());

        var rows = await query
            .OrderBy(x => x.Id)
            .ToListAsync();

        return rows.Select(Map.ToDto).ToList();
    }

    /// <summary>
    /// Runtime execution: apply Scenario/Option FactChanges to a WorkItem facts snapshot (FactsJson), log actions into WorkItemActions,
    /// and then route to the next Kartabl.
    /// </summary>
    [HttpPost("{workItemId:int}/execute-scenario")]
    public async Task<ActionResult<ExecuteScenarioOnWorkItemResponseDto>> ExecuteScenario(int workItemId, [FromBody] ExecuteScenarioOnWorkItemRequestDto? req)
    {
        if (req == null) return BadRequest("Request body is required.");
        if (req.ScenarioId <= 0) return BadRequest("scenarioId is required");

        var wi = await _db.WorkItems.FirstOrDefaultAsync(x => x.Id == workItemId);
        if (wi == null) return NotFound($"WorkItem not found: {workItemId}");

        var scenario = await _db.Scenarios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == req.ScenarioId);
        if (scenario == null) return NotFound($"Scenario not found: {req.ScenarioId}");

        // scenario must be executable in current kartabl
        var allowedKartablIds = await _db.ScenarioKartabls.AsNoTracking()
            .Where(x => x.ScenarioId == scenario.Id)
            .Select(x => x.KartablId)
            .ToListAsync();

        var currentKartablId = req.CurrentKartablId ?? wi.CurrentKartablId;
        if (allowedKartablIds.Count > 0)
        {
            if (!currentKartablId.HasValue)
                return BadRequest("CurrentKartablId is required because this scenario is bound to one or more kartabls.");

            if (!allowedKartablIds.Contains(currentKartablId.Value))
                return BadRequest($"Scenario is not allowed in current kartabl. currentKartablId={currentKartablId.Value}, allowedKartablIds=[{string.Join(",", allowedKartablIds)}].");
        }

        // load facts snapshot
        var beforeFacts = SafeParseFacts(wi.FactsJson);
        var afterFacts = new Dictionary<string, string?>(beforeFacts, StringComparer.OrdinalIgnoreCase);

        // evaluate preconditions
        var preconditionIds = await _db.ScenarioPreconditions.AsNoTracking()
            .Where(x => x.ScenarioId == scenario.Id)
            .Select(x => x.ConditionId)
            .ToListAsync();

        if (preconditionIds.Count > 0)
        {
            var conds = await _db.Conditions.AsNoTracking()
                .Where(c => preconditionIds.Contains(c.Id))
                .ToListAsync();

            var condById = conds.ToDictionary(c => c.Id);
            var missingConditionIds = preconditionIds.Where(id => !condById.ContainsKey(id)).ToList();
            if (missingConditionIds.Count > 0)
                return BadRequest($"Scenario has missing precondition condition(s): [{string.Join(",", missingConditionIds)}].");

            var eval = new SimpleExpressionEvaluator(afterFacts);
            foreach (var cid in preconditionIds)
            {
                var c = condById[cid];
                bool ok;
                try
                {
                    ok = eval.EvaluateBoolean(c.Expression);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Precondition '{c.ConditionKey}' failed: {ex.Message}");
                }

                if (!ok)
                    return BadRequest($"Precondition '{c.ConditionKey}' failed.");
            }
        }

        // apply decision option (if provided)
        int? appliedDecisionOptionId = null;
        List<int> appliedOptionActionIds = new();
        if (req.DecisionOptionId.HasValue)
        {
            var optId = req.DecisionOptionId.Value;
            var opt = await _db.ScenarioDecisionOptions
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == optId);

            if (opt == null)
                return BadRequest($"DecisionOption not found: {optId}");

            // Ensure this option belongs to the scenario (no navigation property in entity)
            var decision = await _db.ScenarioDecisions
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == opt.ScenarioDecisionId);

            if (decision == null)
                return BadRequest($"ScenarioDecision not found for option: {optId}");

            if (decision.ScenarioId != scenario.Id)
                return BadRequest("DecisionOption does not belong to this scenario.");

            // evaluate option conditions
            var optCondIds = SafeParseIds(opt.ConditionIdsJson);
            if (optCondIds.Count > 0)
            {
                var conds = await _db.Conditions.AsNoTracking()
                    .Where(c => optCondIds.Contains(c.Id))
                    .ToListAsync();

                var byId = conds.ToDictionary(c => c.Id);
                var eval = new SimpleExpressionEvaluator(afterFacts);

                foreach (var cid in optCondIds)
                {
                    if (!byId.TryGetValue(cid, out var cond))
                        return BadRequest($"Option condition not found: {cid}");

                    bool ok;
                    try
                    {
                        ok = eval.EvaluateBoolean(cond.Expression);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest($"Option condition '{cond.ConditionKey}' failed: {ex.Message}");
                    }

                    if (!ok)
                        return BadRequest($"Option condition '{cond.ConditionKey}' failed.");
                }
            }

            // apply option fact changes
            var optFactChanges = await _db.DecisionOptionFactChanges.AsNoTracking()
                .Where(fc => fc.ScenarioDecisionOptionId == opt.Id)
                .OrderBy(fc => fc.SortOrder).ThenBy(fc => fc.Id)
                .ToListAsync();

            if (optFactChanges.Count > 0)
            {
                var factIds = optFactChanges.Select(x => x.FactId).Distinct().ToList();
                var factKeyMap = await _db.Facts.AsNoTracking()
                    .Where(f => factIds.Contains(f.Id))
                    .ToDictionaryAsync(f => f.Id, f => f.FactKey);

                foreach (var fc in optFactChanges)
                {
                    if (!factKeyMap.TryGetValue(fc.FactId, out var factKey))
                        return BadRequest($"Option fact change references missing factId={fc.FactId}.");

                    ApplyFactChange(afterFacts, factKey, fc.Op, fc.Value);
                }
            }

            appliedDecisionOptionId = opt.Id;

            // Option action ids (if any)
            appliedOptionActionIds = SafeParseIds(opt.ActionIdsJson);
        }

        // apply scenario-level fact changes
        var factChanges = await _db.ScenarioFactChanges.AsNoTracking()
            .Where(fc => fc.ScenarioId == scenario.Id)
            .OrderBy(fc => fc.SortOrder).ThenBy(fc => fc.Id)
            .ToListAsync();

        if (factChanges.Count > 0)
        {
            var factIds = factChanges.Select(x => x.FactId).Distinct().ToList();
            var factKeyMap = await _db.Facts.AsNoTracking()
                .Where(f => factIds.Contains(f.Id))
                .ToDictionaryAsync(f => f.Id, f => f.FactKey);

            foreach (var fc in factChanges)
            {
                if (!factKeyMap.TryGetValue(fc.FactId, out var factKey))
                    return BadRequest($"Scenario fact change references missing factId={fc.FactId}.");

                ApplyFactChange(afterFacts, factKey, fc.Op, fc.Value);
            }
        }

        // ---- log actions (scenario + option) into runtime WorkItemActions ----
        // Scenario actions
        var scenarioActions = await _db.ScenarioActions.AsNoTracking()
            .Where(a => a.ScenarioId == scenario.Id)
            .OrderBy(a => a.Id)
            .ToListAsync();

        if (scenarioActions.Count > 0)
        {
            var scenarioActionIds = scenarioActions.Select(a => a.ActionId).Distinct().ToList();
            var existingScenarioActionIds = await _db.Actions.AsNoTracking()
                .Where(x => scenarioActionIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync();
            var missingScenarioActionIds = scenarioActionIds.Except(existingScenarioActionIds).ToList();
            if (missingScenarioActionIds.Count > 0)
                return BadRequest($"Scenario action reference(s) not found: [{string.Join(",", missingScenarioActionIds)}].");
        }

        foreach (var a in scenarioActions)
        {
            _db.WorkItemActions.Add(new WorkItemAction
            {
                WorkItemId = wi.Id,
                ActionId = a.ActionId,
                Source = "Scenario",
                SourceScenarioId = scenario.Id,
                SourceDecisionOptionId = null,
                ParamsJson = a.ParamsJson,
                Status = "Pending"
            });
        }

        // Option actions (default params are taken from Actions.DefaultParamsJson)
        if (appliedDecisionOptionId.HasValue && appliedOptionActionIds.Count > 0)
        {
            var defaults = await _db.Actions.AsNoTracking()
                .Where(x => appliedOptionActionIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, x => x.DefaultParamsJson);

            var missingOptionActionIds = appliedOptionActionIds.Distinct().Where(id => !defaults.ContainsKey(id)).ToList();
            if (missingOptionActionIds.Count > 0)
                return BadRequest($"Option action reference(s) not found: [{string.Join(",", missingOptionActionIds)}].");

            foreach (var actionId in appliedOptionActionIds)
            {
                defaults.TryGetValue(actionId, out var def);
                _db.WorkItemActions.Add(new WorkItemAction
                {
                    WorkItemId = wi.Id,
                    ActionId = actionId,
                    Source = "Option",
                    SourceScenarioId = scenario.Id,
                    SourceDecisionOptionId = appliedDecisionOptionId,
                    ParamsJson = def,
                    Status = "Pending"
                });
            }
        }

        // route to next kartabl
        var resolved = await _routing.ResolveAsync(_db, new KartablResolveRequestDto
        {
            OwnerSubdomain = wi.OwnerSubdomain,
            CurrentKartablId = currentKartablId,
            Facts = afterFacts
        }, HttpContext.RequestAborted);

        var beforeKartablId = wi.CurrentKartablId;
        if (resolved.TargetKartablId.HasValue)
        {
            var targetExists = await _db.Kartabls.AsNoTracking().AnyAsync(k => k.Id == resolved.TargetKartablId.Value);
            if (!targetExists)
                return BadRequest($"Matched routing rule points to missing targetKartablId={resolved.TargetKartablId.Value}.");

            wi.CurrentKartablId = resolved.TargetKartablId.Value;
        }

        // Sync denormalized columns from facts snapshot (query-friendly filters)
        // Rasa/Adm uses Asnadm instead of the old CaseStatus fact key. Keep CaseStatus as a fallback for older data.
        if (afterFacts.TryGetValue("Asnadm", out var asnadm))
            wi.CaseStatus = string.IsNullOrWhiteSpace(asnadm) ? null : asnadm;
        else if (afterFacts.TryGetValue("CaseStatus", out var caseStatus))
            wi.CaseStatus = string.IsNullOrWhiteSpace(caseStatus) ? null : caseStatus;
        else
            wi.CaseStatus = null;

        if (afterFacts.TryGetValue("ReferenceNo", out var referenceNo))
            wi.ReferenceNo = string.IsNullOrWhiteSpace(referenceNo) ? null : referenceNo;
        // If the key does not exist, keep previous value (do NOT null it) to avoid accidental wipe.

        if (afterFacts.TryGetValue("CaseId", out var caseId))
            wi.CaseId = string.IsNullOrWhiteSpace(caseId) ? null : caseId;
        // Same behavior: missing key keeps existing value.

        // Keep facts snapshot consistent with current kartabl id
        if (wi.CurrentKartablId.HasValue)
            afterFacts["CurrentKartablId"] = wi.CurrentKartablId.Value.ToString();
        else
            afterFacts.Remove("CurrentKartablId");

        // persist facts snapshot + new kartabl
        wi.FactsJson = JsonSerializer.Serialize(afterFacts);
        await _db.SaveChangesAsync();

        return Ok(new ExecuteScenarioOnWorkItemResponseDto
        {
            WorkItemId = wi.Id,
            ScenarioId = scenario.Id,
            AppliedDecisionOptionId = appliedDecisionOptionId,
            BeforeKartablId = beforeKartablId,
            AfterKartablId = wi.CurrentKartablId,
            MatchedRuleId = resolved.MatchedRuleId,
            MatchedRuleKey = resolved.MatchedRuleKey,
            BeforeFacts = beforeFacts,
            AfterFacts = afterFacts
        });
    }

    private static Dictionary<string, string?> SafeParseFacts(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        try
        {
            var d = JsonSerializer.Deserialize<Dictionary<string, string?>>(json);
            return d == null
                ? new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, string?>(d, StringComparer.OrdinalIgnoreCase);
        }
        catch
        {
            return new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        }
    }

    private static void ApplyFactChange(Dictionary<string, string?> facts, string factKey, FactChangeOp op, string? value)
    {
        switch (op)
        {
            case FactChangeOp.Set:
                facts[factKey] = value ?? string.Empty;
                break;
            case FactChangeOp.Unset:
                facts.Remove(factKey);
                break;
            case FactChangeOp.Inc:
            {
                var cur = facts.TryGetValue(factKey, out var s) ? (s ?? "0") : "0";
                var a = TryParseDecimal(cur);
                var b = TryParseDecimal(value ?? "0");
                facts[factKey] = (a + b).ToString(System.Globalization.CultureInfo.InvariantCulture);
                break;
            }
            case FactChangeOp.Dec:
            {
                var cur = facts.TryGetValue(factKey, out var s) ? (s ?? "0") : "0";
                var a = TryParseDecimal(cur);
                var b = TryParseDecimal(value ?? "0");
                facts[factKey] = (a - b).ToString(System.Globalization.CultureInfo.InvariantCulture);
                break;
            }
            default:
                facts[factKey] = value ?? string.Empty;
                break;
        }
    }

    private static List<int> SafeParseIds(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new List<int>();

        try
        {
            var ids = System.Text.Json.JsonSerializer.Deserialize<List<int>>(json);
            return ids ?? new List<int>();
        }
        catch
        {
            return new List<int>();
        }
    }

    private static decimal TryParseDecimal(string s)
    {
        if (decimal.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var d))
            return d;
        if (decimal.TryParse(s, out d))
            return d;
        return 0m;
    }
}
