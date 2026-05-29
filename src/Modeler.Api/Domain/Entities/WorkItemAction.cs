using System;

namespace Modeler.Api.Domain;

/// <summary>
/// Runtime log/queue of actions to be performed for a WorkItem.
/// This is generated when executing a Scenario (and optionally a Decision Option).
/// </summary>
public sealed class WorkItemAction : BaseEntity
{
    public int WorkItemId { get; set; }
    public int ActionId { get; set; }

    /// <summary>
    /// Where this action came from.
    /// </summary>
    public required string Source { get; set; } // "Scenario" | "Option"

    public int SourceScenarioId { get; set; }
    public int? SourceDecisionOptionId { get; set; }

    public string? ParamsJson { get; set; }

    /// <summary>
    /// Runtime outbox status: Pending | Done | Failed.
    /// </summary>
    public string Status { get; set; } = "Pending";

    public int AttemptCount { get; set; } = 0;
    public DateTime? LastAttemptAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public DateTime? FailedAtUtc { get; set; }
    public string? LastError { get; set; }

    public WorkItem WorkItem { get; set; } = null!;
    public Actions Action { get; set; } = null!;
}
