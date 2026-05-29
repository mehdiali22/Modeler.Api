using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Persistence;
using Modeler.Api.Services;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/kartabl-router")]
public sealed class KartablRouterController : ControllerBase
{
    private readonly ModelerDbContext _db;
    private readonly KartablRoutingEngine _engine;

    public KartablRouterController(ModelerDbContext db, KartablRoutingEngine engine)
    {
        _db = db;
        _engine = engine;
    }

    /// <summary>
    /// Resolve target kartabl based on current facts snapshot and routing rules.
    /// The first matching rule by Priority wins.
    /// </summary>
    [HttpPost("resolve")]
    public async Task<ActionResult<KartablResolveResponseDto>> Resolve([FromBody] KartablResolveRequestDto? req)
    {
        req ??= new KartablResolveRequestDto();
        var resp = await _engine.ResolveAsync(_db, req, HttpContext.RequestAborted);
        return Ok(resp);
    }

    /// <summary>
    /// Resolve target kartabl and write it into ScenarioFactChanges as a Set on a configurable FactKey
    /// (default FactKey: CurrentKartablId).
    ///
    /// Notes:
    /// - This does NOT represent runtime execution; it updates the model definition of the scenario.
    /// - If you don't want persistence, use /resolve.
    /// </summary>
    [HttpPost("apply-to-scenario")]
    public async Task<ActionResult<KartablApplyToScenarioResponseDto>> ApplyToScenario([FromBody] KartablApplyToScenarioRequestDto? req)
    {
        if (req == null) return BadRequest("Request body is required.");
        if (req.ScenarioId <= 0)
            return BadRequest("ScenarioId is required.");

        var resolved = await _engine.ResolveAsync(_db, req, HttpContext.RequestAborted);

        var resp = new KartablApplyToScenarioResponseDto
        {
            TargetKartablId = resolved.TargetKartablId,
            MatchedRuleId = resolved.MatchedRuleId,
            MatchedRuleKey = resolved.MatchedRuleKey,
            Evaluations = resolved.Evaluations,
            Applied = false,
            AppliedScenarioId = req.ScenarioId
        };

        if (!resp.TargetKartablId.HasValue)
            return Ok(resp); // nothing to apply

        var targetExists = await _db.Kartabls.AsNoTracking().AnyAsync(k => k.Id == resp.TargetKartablId.Value);
        if (!targetExists)
            return BadRequest($"Matched routing rule points to missing targetKartablId={resp.TargetKartablId.Value}.");

        var scenarioExists = await _db.Scenarios.AsNoTracking().AnyAsync(s => s.Id == req.ScenarioId);
        if (!scenarioExists)
            return NotFound($"Scenario not found: {req.ScenarioId}");

        var factKey = (req.CurrentKartablFactKey ?? "").Trim();
        if (string.IsNullOrWhiteSpace(factKey))
            factKey = "CurrentKartablId";

        var fact = await _db.Facts.AsNoTracking().FirstOrDefaultAsync(f => f.FactKey == factKey);
        if (fact == null)
            return BadRequest($"Fact not found by FactKey='{factKey}'. Create this Fact first (e.g., FactKey=CurrentKartablId). ");

        // Replace any existing ScenarioFactChange for this ScenarioId+FactId
        var existing = await _db.ScenarioFactChanges
            .Where(x => x.ScenarioId == req.ScenarioId && x.FactId == fact.Id)
            .ToListAsync();

        if (existing.Count > 0)
            _db.ScenarioFactChanges.RemoveRange(existing);

        _db.ScenarioFactChanges.Add(new ScenarioFactChange
        {
            ScenarioId = req.ScenarioId,
            FactId = fact.Id,
            Op = FactChangeOp.Set,
            Value = resp.TargetKartablId.Value.ToString()
        });

        await _db.SaveChangesAsync();

        resp.Applied = true;
        resp.AppliedFactId = fact.Id;
        return Ok(resp);
    }

    /// <summary>
    /// Resolve target kartabl and update a runtime WorkItem (CurrentKartablId and optionally FactsJson).
    /// This is the recommended "apply" for runtime, instead of writing into ScenarioFactChanges.
    /// </summary>
    [HttpPost("apply-to-work-item")]
    public async Task<ActionResult<KartablApplyToWorkItemResponseDto>> ApplyToWorkItem([FromBody] KartablApplyToWorkItemRequestDto? req)
    {
        if (req == null) return BadRequest("Request body is required.");
        if (req.WorkItemId <= 0)
            return BadRequest("WorkItemId is required.");

        var resolved = await _engine.ResolveAsync(_db, req, HttpContext.RequestAborted);

        var resp = new KartablApplyToWorkItemResponseDto
        {
            TargetKartablId = resolved.TargetKartablId,
            MatchedRuleId = resolved.MatchedRuleId,
            MatchedRuleKey = resolved.MatchedRuleKey,
            Evaluations = resolved.Evaluations,
            Applied = false,
            AppliedWorkItemId = req.WorkItemId
        };

        if (!resp.TargetKartablId.HasValue)
            return Ok(resp);

        var targetExists = await _db.Kartabls.AsNoTracking().AnyAsync(k => k.Id == resp.TargetKartablId.Value);
        if (!targetExists)
            return BadRequest($"Matched routing rule points to missing targetKartablId={resp.TargetKartablId.Value}.");

        var wi = await _db.WorkItems.FirstOrDefaultAsync(x => x.Id == req.WorkItemId);
        if (wi == null)
            return NotFound($"WorkItem not found: {req.WorkItemId}");

        wi.CurrentKartablId = resp.TargetKartablId.Value;
        if (req.SaveFactsSnapshot)
        {
            wi.FactsJson = JsonSerializer.Serialize(req.Facts);
        }

        await _db.SaveChangesAsync();
        resp.Applied = true;
        return Ok(resp);
    }

    private Task<KartablResolveResponseDto> ResolveInternal(KartablResolveRequestDto req)
        => _engine.ResolveAsync(_db, req, HttpContext.RequestAborted);

    // SafeParseIds is handled inside KartablRoutingEngine.
}
