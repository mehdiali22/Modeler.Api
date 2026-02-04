namespace Modeler.Api.Domain;

public sealed class ScenarioAction : BaseEntity
{
    public int ScenarioId { get; set; }
    public int ActionId { get; set; }
    public string? ParamsJson { get; set; }

    public Scenario Scenario { get; set; } = null!;
    public Actions Action { get; set; } = null!;
}
