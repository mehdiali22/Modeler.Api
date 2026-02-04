using Modeler.Api.Domain;

namespace Modeler.Api.Dtos;

public sealed class ScenarioDto
{
    public int Id { get; set; }
    public string ScenarioKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }

    public int StageId { get; set; }
    public string? OwnerSubdomain { get; set; }

    public int? TriggerId { get; set; }

    // UI: preconditionIds
    public List<int> PreconditionIds { get; set; } = new();

    // UI: factChanges (scenario-level)
    public List<ScenarioFactChangeDto> FactChanges { get; set; } = new();

    // UI: producedEventIds (scenario-level)
    public List<int> ProducedEventIds { get; set; } = new();

    // UI: actions (scenario-level)
    public List<ScenarioActionRefDto> Actions { get; set; } = new();
}

public sealed class ScenarioActionRefDto
{
    public int ActionId { get; set; }
    public string? ParamsJson { get; set; }
}
