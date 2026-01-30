namespace Modeler.Api.Dtos;

public sealed class ScenarioDecisionOptionDto
{
    public int Id { get; set; }
    public int ScenarioDecisionId { get; set; }
    public string OptionKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public string? ConditionIdsJson { get; set; }
    public string? ActionIdsJson { get; set; }
    public string? ProducedEventIdsJson { get; set; }
}
