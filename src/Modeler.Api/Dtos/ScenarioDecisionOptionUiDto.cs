using System.Collections.Generic;

public sealed class ScenarioDecisionOptionUiDto
{
    public int Id { get; set; }
    public int ScenarioDecisionId { get; set; }
    public string OptionKey { get; set; } = null!;
    public string? TitleFa { get; set; }

    public List<int> ConditionIds { get; set; } = new();
    public List<int> ActionIds { get; set; } = new();
    
}