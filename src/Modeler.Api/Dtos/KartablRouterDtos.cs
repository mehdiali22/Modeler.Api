using System;
using System.Collections.Generic;

namespace Modeler.Api.Dtos;

public class KartablResolveRequestDto
{
    /// <summary>
    /// Optional: limit rules to a subdomain (if your rules are subdomain-scoped).
    /// </summary>
    public string? OwnerSubdomain { get; set; }

    /// <summary>
    /// Optional: used when rules have FromKartablId.
    /// </summary>
    public int? CurrentKartablId { get; set; }

    /// <summary>
    /// Fact values snapshot: key is FactKey, value is string representation.
    /// Examples: {"TotalsValidated":"true", "TotalAmount":"123.45", "CaseStatus":"Ready"}
    /// </summary>
    public Dictionary<string, string?> Facts { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

public sealed class KartablApplyToScenarioRequestDto : KartablResolveRequestDto
{
    /// <summary>
    /// Scenario to update (writes a ScenarioFactChange for the resolved target kartabl).
    /// </summary>
    public int ScenarioId { get; set; }

    /// <summary>
    /// FactKey that represents "current kartabl" in your model. Default: CurrentKartablId
    /// </summary>
    public string CurrentKartablFactKey { get; set; } = "CurrentKartablId";
}

public sealed class KartablApplyToWorkItemRequestDto : KartablResolveRequestDto
{
    /// <summary>
    /// Work item (runtime) to update.
    /// </summary>
    public int WorkItemId { get; set; }

    /// <summary>
    /// If true, writes Facts snapshot into WorkItem.FactsJson.
    /// </summary>
    public bool SaveFactsSnapshot { get; set; } = true;
}

public class KartablResolveResponseDto
{
    public int? TargetKartablId { get; set; }
    public int? MatchedRuleId { get; set; }
    public string? MatchedRuleKey { get; set; }

    /// <summary>
    /// Optional diagnostics for UI/testing.
    /// </summary>
    public List<KartablRuleEvaluationDto> Evaluations { get; set; } = new();
}

public sealed class KartablApplyToScenarioResponseDto : KartablResolveResponseDto
{
    public bool Applied { get; set; }
    public int? AppliedScenarioId { get; set; }
    public int? AppliedFactId { get; set; }
}

public sealed class KartablApplyToWorkItemResponseDto : KartablResolveResponseDto
{
    public bool Applied { get; set; }
    public int? AppliedWorkItemId { get; set; }
}

public sealed class KartablRuleEvaluationDto
{
    public int RuleId { get; set; }
    public string RuleKey { get; set; } = default!;
    public int Priority { get; set; }
    public int? FromKartablId { get; set; }
    public int TargetKartablId { get; set; }

    public bool Passed { get; set; }
    public List<KartablConditionEvaluationDto> Conditions { get; set; } = new();
}

public sealed class KartablConditionEvaluationDto
{
    public int ConditionId { get; set; }
    public string? ConditionKey { get; set; }
    public bool Passed { get; set; }
    public string? Expression { get; set; }
    public string? Error { get; set; }
}
