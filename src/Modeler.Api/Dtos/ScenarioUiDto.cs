using Modeler.Api.Dtos;

public sealed class ScenarioUiDto
{
    public int Id { get; set; }
    public string ScenarioKey { get; set; } = null!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }

    public int StageId { get; set; }
    public int? TriggerId { get; set; }

    public List<int> PreconditionIds { get; set; } = new();

    public List<ScenarioFactChangeDto> FactChanges { get; set; } = new();

    public List<ScenarioDecisionUiDto> Decisions { get; set; } = new();
}
