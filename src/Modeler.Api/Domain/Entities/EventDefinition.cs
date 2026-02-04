namespace Modeler.Api.Domain;
public sealed class EventDefinition : BaseEntity
{
    public string EventKey { get; set; } = null!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }
}
