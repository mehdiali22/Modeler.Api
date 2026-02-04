using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

public sealed record ExportBundle(
    DateTime ExportedAt,
    List<DictionaryTerm> DictionaryTerms,
    List<Artifact> Artifacts,
    List<Fact> Facts,
    List<FactEnumValue> FactEnumValues,
    List<Condition> Conditions,
    List<ConditionFactUsed> ConditionFactUsed,
    List<Actor> Actors,
    List<Actions> Actions,
    List<Process> Processes,
    List<SubProcess> SubProcesses,
    List<Stage> Stages,
    List<Scenario> Scenarios,
    List<ScenarioPrecondition> ScenarioPreconditions,
    List<ScenarioInputArtifact> ScenarioInputArtifacts,
    List<ScenarioFactChange> ScenarioFactChanges,
    List<ScenarioDecision> ScenarioDecisions,
    List<ScenarioDecisionOption> ScenarioDecisionOptions,
    List<DecisionOptionFactChange> DecisionOptionFactChanges
);

public sealed record ValidationIssue(string Entity, int? EntityId, string Message);

[ApiController]
[Route("api/tools")]
public sealed class ToolsController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public ToolsController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet("export")]
    public async Task<ActionResult<ExportBundle>> Export()
    {
        var bundle = new ExportBundle(
            ExportedAt: DateTime.UtcNow,
            DictionaryTerms: await _db.DictionaryTerms.AsNoTracking().ToListAsync(),
            Artifacts: await _db.Artifacts.AsNoTracking().ToListAsync(),
            Facts: await _db.Facts.AsNoTracking().ToListAsync(),
            FactEnumValues: await _db.FactEnumValues.AsNoTracking().ToListAsync(),
            Conditions: await _db.Conditions.AsNoTracking().ToListAsync(),
            ConditionFactUsed: await _db.ConditionFactUsed.AsNoTracking().ToListAsync(),
            Actors: await _db.Actors.AsNoTracking().ToListAsync(),
            Actions: await _db.Actions.AsNoTracking().ToListAsync(),
            Processes: await _db.Processes.AsNoTracking().ToListAsync(),
            SubProcesses: await _db.SubProcesses.AsNoTracking().ToListAsync(),
            Stages: await _db.Stages.AsNoTracking().ToListAsync(),
            Scenarios: await _db.Scenarios.AsNoTracking().ToListAsync(),
            ScenarioPreconditions: await _db.ScenarioPreconditions.AsNoTracking().ToListAsync(),
            ScenarioInputArtifacts: await _db.ScenarioInputArtifacts.AsNoTracking().ToListAsync(),
            ScenarioFactChanges: await _db.ScenarioFactChanges.AsNoTracking().ToListAsync(),
            ScenarioDecisions: await _db.ScenarioDecisions.AsNoTracking().ToListAsync(),
            ScenarioDecisionOptions: await _db.ScenarioDecisionOptions.AsNoTracking().ToListAsync(),
            DecisionOptionFactChanges: await _db.DecisionOptionFactChanges.AsNoTracking().ToListAsync()
        );

        return bundle;
    }

    // mode=replace|upsert
    [HttpPost("import")]
    public async Task<ActionResult> Import([FromQuery] string? mode, [FromBody] ExportBundle input)
    {
        mode = (mode ?? "upsert").Trim().ToLowerInvariant();

        // UI compatibility: merge|overwrite
        mode = mode switch
        {
            "merge" => "upsert",
            "overwrite" => "replace",
            _ => mode
        };

        if (mode == "replace")
        {
            await ReplaceAll(input);
            return Ok();
        }

        // upsert ساده (بدون حفظ id های خاص برای insert جدید)
        UpsertMany(_db.DictionaryTerms, input.DictionaryTerms);
        UpsertMany(_db.Artifacts, input.Artifacts);
        UpsertMany(_db.Facts, input.Facts);
        UpsertMany(_db.FactEnumValues, input.FactEnumValues);
        UpsertMany(_db.Conditions, input.Conditions);

        foreach (var r in input.ConditionFactUsed)
            if (!await _db.ConditionFactUsed.AnyAsync(x => x.ConditionId == r.ConditionId && x.FactId == r.FactId))
                _db.ConditionFactUsed.Add(r);

        UpsertMany(_db.Actors, input.Actors);
        UpsertMany(_db.Actions, input.Actions);
        UpsertMany(_db.Processes, input.Processes);
        UpsertMany(_db.SubProcesses, input.SubProcesses);
        UpsertMany(_db.Stages, input.Stages);
        UpsertMany(_db.Scenarios, input.Scenarios);

        foreach (var r in input.ScenarioPreconditions)
            if (!await _db.ScenarioPreconditions.AnyAsync(x => x.ScenarioId == r.ScenarioId && x.ConditionId == r.ConditionId))
                _db.ScenarioPreconditions.Add(r);

        UpsertMany(_db.ScenarioInputArtifacts, input.ScenarioInputArtifacts);
        UpsertMany(_db.ScenarioFactChanges, input.ScenarioFactChanges);
        UpsertMany(_db.ScenarioDecisions, input.ScenarioDecisions);
        UpsertMany(_db.ScenarioDecisionOptions, input.ScenarioDecisionOptions);
        UpsertMany(_db.DecisionOptionFactChanges, input.DecisionOptionFactChanges);

        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("validate")]
    public async Task<ActionResult<List<ValidationIssue>>> Validate()
    {
        var artifacts = await _db.Artifacts.AsNoTracking().ToListAsync();
        var facts = await _db.Facts.AsNoTracking().ToListAsync();
        var conditions = await _db.Conditions.AsNoTracking().ToListAsync();
        var processes = await _db.Processes.AsNoTracking().ToListAsync();
        var stages = await _db.Stages.AsNoTracking().ToListAsync();
        var scenarios = await _db.Scenarios.AsNoTracking().ToListAsync();
        var actors = await _db.Actors.AsNoTracking().ToListAsync();
        var actions = await _db.Actions.AsNoTracking().ToListAsync();
        var opts = await _db.ScenarioDecisionOptions.AsNoTracking().ToListAsync();
        var optFcs = await _db.DecisionOptionFactChanges.AsNoTracking().ToListAsync();
        var scFcs = await _db.ScenarioFactChanges.AsNoTracking().ToListAsync();
        var scInputs = await _db.ScenarioInputArtifacts.AsNoTracking().ToListAsync();
        var scDecisions = await _db.ScenarioDecisions.AsNoTracking().ToListAsync();
        var scPre = await _db.ScenarioPreconditions.AsNoTracking().ToListAsync();
        var cfUsed = await _db.ConditionFactUsed.AsNoTracking().ToListAsync();
        var factEnums = await _db.FactEnumValues.AsNoTracking().ToListAsync();
        var subps = await _db.SubProcesses.AsNoTracking().ToListAsync();

        var artIds = artifacts.Select(x => x.Id).ToHashSet();
        var factIds = facts.Select(x => x.Id).ToHashSet();
        var condIds = conditions.Select(x => x.Id).ToHashSet();
        var procIds = processes.Select(x => x.Id).ToHashSet();
        var stageIds = stages.Select(x => x.Id).ToHashSet();
        var scIds = scenarios.Select(x => x.Id).ToHashSet();
        var actorIds = actors.Select(x => x.Id).ToHashSet();
        var optIds = opts.Select(x => x.Id).ToHashSet();
        var scDecisionIds = scDecisions.Select(x => x.Id).ToHashSet();

        var issues = new List<ValidationIssue>();

        foreach (var f in facts)
            if (!artIds.Contains(f.ArtifactId))
                issues.Add(new("Fact", f.Id, $"artifactId '{f.ArtifactId}' not found"));

        foreach (var fe in factEnums)
            if (!factIds.Contains(fe.FactId))
                issues.Add(new("FactEnumValue", fe.Id, $"factId '{fe.FactId}' not found"));

        foreach (var cfu in cfUsed)
        {
            if (!condIds.Contains(cfu.ConditionId))
                issues.Add(new("ConditionFactUsed", null, $"conditionId '{cfu.ConditionId}' not found"));
            if (!factIds.Contains(cfu.FactId))
                issues.Add(new("ConditionFactUsed", null, $"factId '{cfu.FactId}' not found"));
        }

        foreach (var st in stages)
            if (!procIds.Contains(st.ProcessId))
                issues.Add(new("Stage", st.Id, $"processId '{st.ProcessId}' not found"));

        foreach (var sp in subps)
            if (!procIds.Contains(sp.ProcessId))
                issues.Add(new("SubProcess", sp.Id, $"processId '{sp.ProcessId}' not found"));

        foreach (var sc in scenarios)
            if (!stageIds.Contains(sc.StageId))
                issues.Add(new("Scenario", sc.Id, $"stageId '{sc.StageId}' not found"));

        foreach (var pre in scPre)
        {
            if (!scIds.Contains(pre.ScenarioId))
                issues.Add(new("ScenarioPrecondition", null, $"scenarioId '{pre.ScenarioId}' not found"));
            if (!condIds.Contains(pre.ConditionId))
                issues.Add(new("ScenarioPrecondition", null, $"conditionId '{pre.ConditionId}' not found"));
        }

        foreach (var inp in scInputs)
        {
            if (!scIds.Contains(inp.ScenarioId))
                issues.Add(new("ScenarioInputArtifact", inp.Id, $"scenarioId '{inp.ScenarioId}' not found"));
            if (!artIds.Contains(inp.ArtifactId))
                issues.Add(new("ScenarioInputArtifact", inp.Id, $"artifactId '{inp.ArtifactId}' not found"));
        }

        foreach (var fc in scFcs)
        {
            if (!scIds.Contains(fc.ScenarioId))
                issues.Add(new("ScenarioFactChange", fc.Id, $"scenarioId '{fc.ScenarioId}' not found"));
            if (!factIds.Contains(fc.FactId))
                issues.Add(new("ScenarioFactChange", fc.Id, $"factId '{fc.FactId}' not found"));
        }

        foreach (var ac in actions)
        {
            if (ac.TargetArtifactId.HasValue && !artIds.Contains(ac.TargetArtifactId.Value))
                issues.Add(new("Actions", ac.Id, $"targetArtifactId '{ac.TargetArtifactId}' not found"));
            if (ac.ExecutorActorId.HasValue && !actorIds.Contains(ac.ExecutorActorId.Value))
                issues.Add(new("Actions", ac.Id, $"executorActorId '{ac.ExecutorActorId}' not found"));
        }

        foreach (var d in scDecisions)
            if (!scIds.Contains(d.ScenarioId))
                issues.Add(new("ScenarioDecision", d.Id, $"scenarioId '{d.ScenarioId}' not found"));

        foreach (var o in opts)
            if (!scDecisionIds.Contains(o.ScenarioDecisionId))
                issues.Add(new("ScenarioDecisionOption", o.Id, $"scenarioDecisionId '{o.ScenarioDecisionId}' not found"));

        foreach (var ofc in optFcs)
        {
            if (!optIds.Contains(ofc.ScenarioDecisionOptionId))
                issues.Add(new("DecisionOptionFactChange", ofc.Id, $"scenarioDecisionOptionId '{ofc.ScenarioDecisionOptionId}' not found"));
            if (!factIds.Contains(ofc.FactId))
                issues.Add(new("DecisionOptionFactChange", ofc.Id, $"factId '{ofc.FactId}' not found"));
        }

        return issues;
    }

    private async Task ReplaceAll(ExportBundle input)
    {
        // delete all (FK order: children first)
        _db.DecisionOptionFactChanges.RemoveRange(_db.DecisionOptionFactChanges);
        _db.ScenarioDecisionOptions.RemoveRange(_db.ScenarioDecisionOptions);
        _db.ScenarioDecisions.RemoveRange(_db.ScenarioDecisions);
        _db.ScenarioFactChanges.RemoveRange(_db.ScenarioFactChanges);
        _db.ScenarioInputArtifacts.RemoveRange(_db.ScenarioInputArtifacts);
        _db.ScenarioPreconditions.RemoveRange(_db.ScenarioPreconditions);
        _db.ConditionFactUsed.RemoveRange(_db.ConditionFactUsed);
        _db.Scenarios.RemoveRange(_db.Scenarios);
        _db.Stages.RemoveRange(_db.Stages);
        _db.SubProcesses.RemoveRange(_db.SubProcesses);
        _db.Processes.RemoveRange(_db.Processes);
        _db.Actions.RemoveRange(_db.Actions);
        _db.Actors.RemoveRange(_db.Actors);
        _db.Conditions.RemoveRange(_db.Conditions);
        _db.FactEnumValues.RemoveRange(_db.FactEnumValues);
        _db.Facts.RemoveRange(_db.Facts);
        _db.Artifacts.RemoveRange(_db.Artifacts);
        _db.DictionaryTerms.RemoveRange(_db.DictionaryTerms);
        await _db.SaveChangesAsync();

        // insert with identity values preserved (SET IDENTITY_INSERT)
        using var tx = await _db.Database.BeginTransactionAsync();

        await InsertWithIdentity("DictionaryTerms", input.DictionaryTerms);
        await InsertWithIdentity("Artifacts", input.Artifacts);
        await InsertWithIdentity("Facts", input.Facts);
        await InsertWithIdentity("FactEnumValues", input.FactEnumValues);
        await InsertWithIdentity("Conditions", input.Conditions);

        _db.ConditionFactUsed.AddRange(input.ConditionFactUsed);
        await _db.SaveChangesAsync();

        await InsertWithIdentity("Actors", input.Actors);
        await InsertWithIdentity("Actions", input.Actions);
        await InsertWithIdentity("Processes", input.Processes);
        await InsertWithIdentity("SubProcesses", input.SubProcesses);
        await InsertWithIdentity("Stages", input.Stages);
        await InsertWithIdentity("Scenarios", input.Scenarios);

        _db.ScenarioPreconditions.AddRange(input.ScenarioPreconditions);
        await _db.SaveChangesAsync();

        await InsertWithIdentity("ScenarioInputArtifacts", input.ScenarioInputArtifacts);
        await InsertWithIdentity("ScenarioFactChanges", input.ScenarioFactChanges);
        await InsertWithIdentity("ScenarioDecisions", input.ScenarioDecisions);
        await InsertWithIdentity("ScenarioDecisionOptions", input.ScenarioDecisionOptions);
        await InsertWithIdentity("DecisionOptionFactChanges", input.DecisionOptionFactChanges);

        await tx.CommitAsync();
    }

    private async Task InsertWithIdentity<TEntity>(string tableName, List<TEntity> rows) where TEntity : BaseEntity
    {
        if (rows.Count == 0) return;

        // اگر همه Id=0 باشه، اصلاً identity_insert لازم نیست
        var hasExplicit = rows.Any(x => x.Id > 0);
        if (!hasExplicit)
        {
            _db.Set<TEntity>().AddRange(rows);
            await _db.SaveChangesAsync();
            return;
        }

        await _db.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.{tableName} ON;");
        _db.Set<TEntity>().AddRange(rows);
        await _db.SaveChangesAsync();
        await _db.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT dbo.{tableName} OFF;");
    }

    private static void UpsertMany<TEntity>(DbSet<TEntity> set, IEnumerable<TEntity> rows)
        where TEntity : BaseEntity
    {
        foreach (var r in rows)
        {
            if (r.Id <= 0)
            {
                // insert new identity
                r.Id = 0;
                set.Add(r);
                continue;
            }

            var exists = set.Any(x => x.Id == r.Id);
            set.Attach(r);
            set.Entry(r).State = exists ? EntityState.Modified : EntityState.Added;
        }
    }
}
