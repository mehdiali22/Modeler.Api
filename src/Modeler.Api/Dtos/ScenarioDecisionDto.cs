namespace Modeler.Api.Dtos;

public sealed class ScenarioDecisionDto
{
    public int Id { get; set; }
    public int ScenarioId { get; set; }
    public string DecisionKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public string? UiActionKey { get; set; }
}
