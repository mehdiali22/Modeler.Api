using Modeler.Api.Dtos;

public sealed class ScenarioDecisionUiDto
{
    public int Id { get; set; }
    public int ScenarioId { get; set; }
    public string DecisionKey { get; set; } = null!;
    public string? TitleFa { get; set; }

    public List<ScenarioDecisionOptionUiDto> Options { get; set; } = new();
}