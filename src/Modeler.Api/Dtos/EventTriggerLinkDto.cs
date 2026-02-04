namespace Modeler.Api.Dtos;

public sealed class EventTriggerLinkDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int TriggerId { get; set; }
}
