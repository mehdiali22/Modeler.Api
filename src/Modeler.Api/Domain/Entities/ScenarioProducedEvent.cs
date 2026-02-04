namespace Modeler.Api.Domain;

public sealed class ScenarioProducedEvent : BaseEntity
{
    public int ScenarioId { get; set; }
    public int EventId { get; set; }

    public Scenario Scenario { get; set; } = null!;
    public EventDefinition Event { get; set; } = null!;
}
