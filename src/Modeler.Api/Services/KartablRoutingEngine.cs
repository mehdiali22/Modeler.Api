using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Dtos;
using Modeler.Api.Domain;
using Modeler.Api.Persistence;

namespace Modeler.Api.Services;

/// <summary>
/// Evaluates KartablRoutingRules against a facts snapshot and returns the first matching rule (by Priority).
///
/// Notes:
/// - Conditions are referenced via ConditionIdsJson (JSON array of ints).
/// - Each Condition.Expression is evaluated using <see cref="SimpleExpressionEvaluator"/>.
/// - If rule.OwnerSubdomain is set, it must match request.OwnerSubdomain; if rule.OwnerSubdomain is null/empty, it's treated as wildcard.
/// - If rule.FromKartablId is set, it must match request.CurrentKartablId.
/// </summary>
public sealed class KartablRoutingEngine
{
    public async Task<KartablResolveResponseDto> ResolveAsync(ModelerDbContext db, KartablResolveRequestDto req, CancellationToken ct)
    {
        req ??= new KartablResolveRequestDto();
        req.Facts ??= new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        var owner = (req.OwnerSubdomain ?? string.Empty).Trim();

        var q = db.KartablRoutingRules
            .AsNoTracking()
            .OrderBy(r => r.Priority)
            .ThenBy(r => r.Id)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(owner))
        {
            q = q.Where(r => r.OwnerSubdomain == null || r.OwnerSubdomain == "" || r.OwnerSubdomain == owner);
        }

        // If CurrentKartablId is null, rules with FromKartablId cannot pass; keep them but mark failed in evaluation.
        var rules = await q.ToListAsync(ct);

        // Collect all condition ids across rules
        var ruleCondIds = new Dictionary<int, List<int>>();
        var allCondIds = new HashSet<int>();
        foreach (var r in rules)
        {
            var ids = SafeParseIds(r.ConditionIdsJson);
            ruleCondIds[r.Id] = ids;
            foreach (var id in ids) allCondIds.Add(id);
        }

        var conds = allCondIds.Count == 0
            ? new List<Condition>()
            : await db.Conditions.AsNoTracking().Where(c => allCondIds.Contains(c.Id)).ToListAsync(ct);
        var condById = conds.ToDictionary(c => c.Id);

        var evaluator = new SimpleExpressionEvaluator(req.Facts);

        var resp = new KartablResolveResponseDto();

        foreach (var r in rules)
        {
            var evalDto = new KartablRuleEvaluationDto
            {
                RuleId = r.Id,
                RuleKey = r.RuleKey,
                Priority = r.Priority,
                FromKartablId = r.FromKartablId,
                TargetKartablId = r.TargetKartablId,
            };

            // FromKartabl filter
            if (r.FromKartablId.HasValue)
            {
                if (!req.CurrentKartablId.HasValue || req.CurrentKartablId.Value != r.FromKartablId.Value)
                {
                    evalDto.Passed = false;
                    resp.Evaluations.Add(evalDto);
                    continue;
                }
            }

            var ids = ruleCondIds.TryGetValue(r.Id, out var tmp) ? tmp : new List<int>();
            var allPassed = true;
            foreach (var cid in ids)
            {
                var cEval = new KartablConditionEvaluationDto { ConditionId = cid };
                if (!condById.TryGetValue(cid, out var cond))
                {
                    cEval.Passed = false;
                    cEval.Error = "Condition not found";
                    allPassed = false;
                    evalDto.Conditions.Add(cEval);
                    continue;
                }

                cEval.ConditionKey = cond.ConditionKey;
                cEval.Expression = cond.Expression;
                try
                {
                    var ok = evaluator.EvaluateBoolean(cond.Expression);
                    cEval.Passed = ok;
                    if (!ok) allPassed = false;
                }
                catch (Exception ex)
                {
                    cEval.Passed = false;
                    cEval.Error = ex.Message;
                    allPassed = false;
                }

                evalDto.Conditions.Add(cEval);
            }

            evalDto.Passed = allPassed;
            resp.Evaluations.Add(evalDto);

            if (allPassed)
            {
                resp.TargetKartablId = r.TargetKartablId;
                resp.MatchedRuleId = r.Id;
                resp.MatchedRuleKey = r.RuleKey;
                break;
            }
        }

        return resp;
    }

    private static List<int> SafeParseIds(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return new List<int>();
        try
        {
            var arr = JsonSerializer.Deserialize<List<int>>(json);
            return arr ?? new List<int>();
        }
        catch
        {
            return new List<int>();
        }
    }
}
