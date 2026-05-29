using System;

namespace Modeler.Api.Dtos;

public sealed class WorkItemActionDto
{
    public int Id { get; set; }
    public int WorkItemId { get; set; }
    public int ActionId { get; set; }
    public string Source { get; set; } = "";
    public int SourceScenarioId { get; set; }
    public int? SourceDecisionOptionId { get; set; }
    public string? ParamsJson { get; set; }

    public string Status { get; set; } = "Pending";
    public int AttemptCount { get; set; }
    public DateTime? LastAttemptAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public DateTime? FailedAtUtc { get; set; }
    public string? LastError { get; set; }
}
