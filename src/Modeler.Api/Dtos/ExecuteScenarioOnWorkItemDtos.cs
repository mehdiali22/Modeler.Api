using System.Collections.Generic;

namespace Modeler.Api.Dtos;

public sealed class ExecuteScenarioOnWorkItemRequestDto
{
    public int ScenarioId { get; set; }

    /// <summary>
    /// Optional chosen decision option to apply before scenario-level fact changes.
    /// The option must belong to a decision of the given scenario.
    /// </summary>
    public int? DecisionOptionId { get; set; }

    /// <summary>
    /// Optional override for current kartabl id. If omitted, WorkItem.CurrentKartablId is used.
    /// </summary>
    public int? CurrentKartablId { get; set; }
}

public sealed class ExecuteScenarioOnWorkItemResponseDto
{
    public int? AppliedDecisionOptionId { get; set; }

    public int WorkItemId { get; set; }
    public int ScenarioId { get; set; }

    /// <summary>
    /// Optional chosen decision option to apply before scenario-level fact changes.
    /// The option must belong to a decision of the given scenario.
    /// </summary>
    public int? DecisionOptionId { get; set; }

    public int? BeforeKartablId { get; set; }
    public int? AfterKartablId { get; set; }

    public int? MatchedRuleId { get; set; }
    public string? MatchedRuleKey { get; set; }

    public Dictionary<string, string?> BeforeFacts { get; set; } = new();
    public Dictionary<string, string?> AfterFacts { get; set; } = new();
}
