using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using EntityStateModel = Modeler.Api.Domain.EntityStatee;
using Modeler.Api.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modeler.Api.Controllers;

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

            Kartabls: await _db.Kartabls.AsNoTracking().ToListAsync(),
            KartablRoutingRules: await _db.KartablRoutingRules.AsNoTracking().ToListAsync(),

            WorkItems: await _db.WorkItems.AsNoTracking().ToListAsync(),

            WorkItemActions: await _db.WorkItemActions.AsNoTracking().ToListAsync(),

            EntityStates: await _db.EntityStates.AsNoTracking().ToListAsync(),
            ActionStateTransitions: await _db.ActionStateTransitions.AsNoTracking().ToListAsync(),
                        
            Processes: await _db.Processes.AsNoTracking().ToListAsync(),
            SubProcesses: await _db.SubProcesses.AsNoTracking().ToListAsync(),
            Stages: await _db.Stages.AsNoTracking().ToListAsync(),
            Scenarios: await _db.Scenarios.AsNoTracking().ToListAsync(),
            ScenarioPreconditions: await _db.ScenarioPreconditions.AsNoTracking().ToListAsync(),
            ScenarioInputArtifacts: await _db.ScenarioInputArtifacts.AsNoTracking().ToListAsync(),
            ScenarioFactChanges: await _db.ScenarioFactChanges.AsNoTracking().ToListAsync(),

            ScenarioKartabls: await _db.ScenarioKartabls.AsNoTracking().ToListAsync(),
             
            // ✅ NEW: scenario-level actions
            ScenarioActions: await _db.ScenarioActions.AsNoTracking().ToListAsync(),

            ScenarioDecisions: await _db.ScenarioDecisions.AsNoTracking().ToListAsync(),
            ScenarioDecisionOptions: await _db.ScenarioDecisionOptions.AsNoTracking().ToListAsync(),
            DecisionOptionFactChanges: await _db.DecisionOptionFactChanges.AsNoTracking().ToListAsync()
        );

        return bundle;
    }

    // mode=replace|upsert  (also accepts UI: overwrite|merge)
    [HttpPost("import")]
    public async Task<ActionResult> Import([FromQuery] string? mode, [FromBody] ExportBundle input)
    {
        mode = (mode ?? "upsert").Trim().ToLowerInvariant();

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

        if (mode != "upsert")
            return BadRequest(new { error = "mode must be replace|upsert (also accepts overwrite|merge)" });

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

        UpsertMany(_db.Kartabls, input.Kartabls);
        UpsertMany(_db.KartablRoutingRules, input.KartablRoutingRules);

        // runtime-ish; depends on Kartabls
        UpsertMany(_db.WorkItems, input.WorkItems);

        // runtime-ish; depends on WorkItems + Actions
        UpsertMany(_db.WorkItemActions, input.WorkItemActions ?? new List<WorkItemAction>());

        UpsertMany(_db.EntityStates, input.EntityStates ?? new List<EntityStateModel>());
        UpsertMany(_db.ActionStateTransitions, input.ActionStateTransitions ?? new List<ActionStateTransition>());

        
        UpsertMany(_db.Processes, input.Processes);
        UpsertMany(_db.SubProcesses, input.SubProcesses);
        UpsertMany(_db.Stages, input.Stages);
        UpsertMany(_db.Scenarios, input.Scenarios);

        foreach (var r in input.ScenarioPreconditions)
            if (!await _db.ScenarioPreconditions.AnyAsync(x => x.ScenarioId == r.ScenarioId && x.ConditionId == r.ConditionId))
                _db.ScenarioPreconditions.Add(r);

        UpsertMany(_db.ScenarioInputArtifacts, input.ScenarioInputArtifacts);
        UpsertMany(_db.ScenarioFactChanges, input.ScenarioFactChanges);

        foreach (var r in input.ScenarioKartabls)
            if (!await _db.ScenarioKartabls.AnyAsync(x => x.ScenarioId == r.ScenarioId && x.KartablId == r.KartablId))
                _db.ScenarioKartabls.Add(r);

       
        // ✅ NEW: needs scenarios + actions
        UpsertMany(_db.ScenarioActions, input.ScenarioActions);

        UpsertMany(_db.ScenarioDecisions, input.ScenarioDecisions);
        UpsertMany(_db.ScenarioDecisionOptions, input.ScenarioDecisionOptions);
        UpsertMany(_db.DecisionOptionFactChanges, input.DecisionOptionFactChanges);

        // links last        

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

        var kartabls = await _db.Kartabls.AsNoTracking().ToListAsync();
        var kartablRules = await _db.KartablRoutingRules.AsNoTracking().ToListAsync();

        var workItems = await _db.WorkItems.AsNoTracking().ToListAsync();

        var workItemActions = await _db.WorkItemActions.AsNoTracking().ToListAsync();

        
        var opts = await _db.ScenarioDecisionOptions.AsNoTracking().ToListAsync();
        var optFcs = await _db.DecisionOptionFactChanges.AsNoTracking().ToListAsync();
        var scFcs = await _db.ScenarioFactChanges.AsNoTracking().ToListAsync();
        var scInputs = await _db.ScenarioInputArtifacts.AsNoTracking().ToListAsync();
        var scDecisions = await _db.ScenarioDecisions.AsNoTracking().ToListAsync();
        var scPre = await _db.ScenarioPreconditions.AsNoTracking().ToListAsync();
        var cfUsed = await _db.ConditionFactUsed.AsNoTracking().ToListAsync();
        var factEnums = await _db.FactEnumValues.AsNoTracking().ToListAsync();
        var subps = await _db.SubProcesses.AsNoTracking().ToListAsync();
                
        var scActs = await _db.ScenarioActions.AsNoTracking().ToListAsync(); // ✅ NEW

        var scKartabls = await _db.ScenarioKartabls.AsNoTracking().ToListAsync();

        var artIds = artifacts.Select(x => x.Id).ToHashSet();
        var factIds = facts.Select(x => x.Id).ToHashSet();
        var condIds = conditions.Select(x => x.Id).ToHashSet();
        var procIds = processes.Select(x => x.Id).ToHashSet();
        var stageIds = stages.Select(x => x.Id).ToHashSet();
        var subProcessIds = subps.Select(x => x.Id).ToHashSet();
        var scIds = scenarios.Select(x => x.Id).ToHashSet();
        var actorIds = actors.Select(x => x.Id).ToHashSet();
        var actionIds = actions.Select(x => x.Id).ToHashSet(); // ✅ NEW for scenario actions

        var workItemIds = workItems.Select(x => x.Id).ToHashSet();

        var kartablIds = kartabls.Select(x => x.Id).ToHashSet();

        var optIds = opts.Select(x => x.Id).ToHashSet();
        var scDecisionIds = scDecisions.Select(x => x.Id).ToHashSet();


        var issues = new List<ValidationIssue>();

        // Required standard facts for the Status+Kartabl model
        if (!facts.Any(f => f.FactKey == "Asnadm"))
            issues.Add(new ValidationIssue("Fact", null, "ERROR: Required Fact is missing: FactKey=Asnadm"));

        if (!facts.Any(f => f.FactKey == "CurrentKartablId"))
            issues.Add(new ValidationIssue("Fact", null, "ERROR: Required Fact is missing: FactKey=CurrentKartablId"));

        foreach (var f in facts)
            if (!artIds.Contains(f.ArtifactId))
                issues.Add(new("Fact", f.Id, $"artifactId '{f.ArtifactId}' not found"));

        var validWorkItemActionStatuses = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Pending", "Done", "Failed" };

        foreach (var wia in workItemActions)
        {
            if (!workItemIds.Contains(wia.WorkItemId))
                issues.Add(new("WorkItemAction", wia.Id, $"workItemId '{wia.WorkItemId}' not found"));
            if (!actionIds.Contains(wia.ActionId))
                issues.Add(new("WorkItemAction", wia.Id, $"actionId '{wia.ActionId}' not found"));
            if (string.IsNullOrWhiteSpace(wia.Status) || !validWorkItemActionStatuses.Contains(wia.Status))
                issues.Add(new("WorkItemAction", wia.Id, $"invalid status '{wia.Status}'. Allowed: Pending, Done, Failed"));
            if (wia.Status == "Failed" && string.IsNullOrWhiteSpace(wia.LastError))
                issues.Add(new("WorkItemAction", wia.Id, "WARN: failed action has no LastError"));
        }

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
        {
            if (!procIds.Contains(st.ProcessId))
                issues.Add(new("Stage", st.Id, $"processId '{st.ProcessId}' not found"));
            if (st.SubProcessId.HasValue && !subProcessIds.Contains(st.SubProcessId.Value))
                issues.Add(new("Stage", st.Id, $"subProcessId '{st.SubProcessId}' not found"));
            var sp = st.SubProcessId.HasValue ? subps.FirstOrDefault(x => x.Id == st.SubProcessId.Value) : null;
            if (sp != null && sp.ProcessId != st.ProcessId)
                issues.Add(new("Stage", st.Id, "subProcessId belongs to another process"));
        }

        foreach (var sp in subps)
            if (!procIds.Contains(sp.ProcessId))
                issues.Add(new("SubProcess", sp.Id, $"processId '{sp.ProcessId}' not found"));

        

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

       

 

        // ✅ NEW: ScenarioActions validation
        foreach (var sa in scActs)
        {
            if (!scIds.Contains(sa.ScenarioId))
                issues.Add(new("ScenarioAction", sa.Id, $"scenarioId '{sa.ScenarioId}' not found"));
            if (!actionIds.Contains(sa.ActionId))
                issues.Add(new("ScenarioAction", sa.Id, $"actionId '{sa.ActionId}' not found"));
        }

        foreach (var k in kartabls)
        {
            if (string.IsNullOrWhiteSpace(k.KartablKey))
                issues.Add(new("Kartabl", k.Id, "kartablKey is empty"));
        }

        foreach (var rr in kartablRules)
        {
            if (rr.FromKartablId.HasValue && !kartablIds.Contains(rr.FromKartablId.Value))
                issues.Add(new("KartablRoutingRule", rr.Id, $"fromKartablId '{rr.FromKartablId}' not found"));
            if (!kartablIds.Contains(rr.TargetKartablId))
                issues.Add(new("KartablRoutingRule", rr.Id, $"targetKartablId '{rr.TargetKartablId}' not found"));
        }

        foreach (var wi in workItems)
        {
            if (string.IsNullOrWhiteSpace(wi.WorkItemKey))
                issues.Add(new("WorkItem", wi.Id, "workItemKey is empty"));
            if (!kartablIds.Contains((int)wi.CurrentKartablId))
                issues.Add(new("WorkItem", wi.Id, $"currentKartablId '{wi.CurrentKartablId}' not found"));
        }

        foreach (var sk in scKartabls)
        {
            if (!scIds.Contains(sk.ScenarioId))
                issues.Add(new("ScenarioKartabl", null, $"scenarioId '{sk.ScenarioId}' not found"));
            if (!kartablIds.Contains(sk.KartablId))
                issues.Add(new("ScenarioKartabl", null, $"kartablId '{sk.KartablId}' not found"));
        }

        return issues;
    }

    private async Task ReplaceAll(ExportBundle input)
    {
        // children first
        _db.DecisionOptionFactChanges.RemoveRange(_db.DecisionOptionFactChanges);
        _db.ScenarioDecisionOptions.RemoveRange(_db.ScenarioDecisionOptions);
        _db.ScenarioDecisions.RemoveRange(_db.ScenarioDecisions);

        _db.ScenarioActions.RemoveRange(_db.ScenarioActions);               // ✅ NEW

        // runtime-ish children
        _db.WorkItemActions.RemoveRange(_db.WorkItemActions);
        _db.ActionStateTransitions.RemoveRange(_db.ActionStateTransitions);
        _db.EntityStates.RemoveRange(_db.EntityStates);
        

        _db.ScenarioKartabls.RemoveRange(_db.ScenarioKartabls);

        _db.ScenarioFactChanges.RemoveRange(_db.ScenarioFactChanges);
        _db.ScenarioInputArtifacts.RemoveRange(_db.ScenarioInputArtifacts);
        _db.ScenarioPreconditions.RemoveRange(_db.ScenarioPreconditions);
        _db.ConditionFactUsed.RemoveRange(_db.ConditionFactUsed);

        _db.Scenarios.RemoveRange(_db.Scenarios);
        _db.Stages.RemoveRange(_db.Stages);
        _db.SubProcesses.RemoveRange(_db.SubProcesses);
        _db.Processes.RemoveRange(_db.Processes);
        
        _db.KartablRoutingRules.RemoveRange(_db.KartablRoutingRules);

        // runtime-ish; depends on Kartabls
        _db.WorkItems.RemoveRange(_db.WorkItems);
        _db.Kartabls.RemoveRange(_db.Kartabls);

        _db.Actions.RemoveRange(_db.Actions);
        _db.Actors.RemoveRange(_db.Actors);
        _db.Conditions.RemoveRange(_db.Conditions);
        _db.FactEnumValues.RemoveRange(_db.FactEnumValues);
        _db.Facts.RemoveRange(_db.Facts);
        _db.Artifacts.RemoveRange(_db.Artifacts);
        _db.DictionaryTerms.RemoveRange(_db.DictionaryTerms);
        await _db.SaveChangesAsync();

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

        await InsertWithIdentity("Kartabls", input.Kartabls);
        await InsertWithIdentity("KartablRoutingRules", input.KartablRoutingRules);

        // runtime-ish; after Kartabls exist
        await InsertWithIdentity("WorkItems", input.WorkItems);

        // runtime-ish; after WorkItems + Actions exist
        await InsertWithIdentity("WorkItemActions", input.WorkItemActions ?? new List<WorkItemAction>());

        await InsertWithIdentity("EntityStates", input.EntityStates ?? new List<EntityStateModel>());
        await InsertWithIdentity("ActionStateTransitions", input.ActionStateTransitions ?? new List<ActionStateTransition>());
                
        await InsertWithIdentity("Processes", input.Processes);
        await InsertWithIdentity("SubProcesses", input.SubProcesses);
        await InsertWithIdentity("Stages", input.Stages);
        await InsertWithIdentity("Scenarios", input.Scenarios);

        _db.ScenarioPreconditions.AddRange(input.ScenarioPreconditions);
        await _db.SaveChangesAsync();

        await InsertWithIdentity("ScenarioInputArtifacts", input.ScenarioInputArtifacts);
        await InsertWithIdentity("ScenarioFactChanges", input.ScenarioFactChanges);

        _db.ScenarioKartabls.AddRange(input.ScenarioKartabls);
        await _db.SaveChangesAsync();
               

        // ✅ NEW: after Scenarios + Actions exist
        await InsertWithIdentity("ScenarioActions", input.ScenarioActions);

        await InsertWithIdentity("ScenarioDecisions", input.ScenarioDecisions);
        await InsertWithIdentity("ScenarioDecisionOptions", input.ScenarioDecisionOptions);
        await InsertWithIdentity("DecisionOptionFactChanges", input.DecisionOptionFactChanges);

        await tx.CommitAsync();
    }

    private async Task InsertWithIdentity<TEntity>(string tableName, List<TEntity> rows) where TEntity : BaseEntity
    {
        if (rows.Count == 0) return;

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

    List<Kartabl> Kartabls,
    List<KartablRoutingRule> KartablRoutingRules,

    List<WorkItem> WorkItems,

    List<WorkItemAction>? WorkItemActions,

    List<EntityStateModel>? EntityStates,
    List<ActionStateTransition>? ActionStateTransitions,

    List<Process> Processes,
    List<SubProcess> SubProcesses,
    List<Stage> Stages,
    List<Scenario> Scenarios,
    List<ScenarioPrecondition> ScenarioPreconditions,
    List<ScenarioInputArtifact> ScenarioInputArtifacts,
    List<ScenarioFactChange> ScenarioFactChanges,

    List<ScenarioKartabl> ScenarioKartabls, 

    // ✅ NEW
    List<ScenarioAction> ScenarioActions,

    List<ScenarioDecision> ScenarioDecisions,
    List<ScenarioDecisionOption> ScenarioDecisionOptions,
    List<DecisionOptionFactChange> DecisionOptionFactChanges
);

public sealed record ValidationIssue(string Entity, int? EntityId, string Message);
