namespace Modeler.Api.Domain;

public sealed class EventTriggerLink : BaseEntity
{
    public int EventId { get; set; }
    public int TriggerId { get; set; }

    public EventDefinition Event { get; set; } = null!;
    public TriggerDefinition Trigger { get; set; } = null!;
}
