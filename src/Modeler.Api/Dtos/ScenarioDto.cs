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

    // NEW (UI expects)
    public int? TriggerId { get; set; }

    // NEW (flattened from ScenarioPreconditions)
    public List<int> PreconditionIds { get; set; } = new();

    // NEW (flattened from ScenarioFactChanges)
    public List<ScenarioFactChangeDto> FactChanges { get; set; } = new();
}
