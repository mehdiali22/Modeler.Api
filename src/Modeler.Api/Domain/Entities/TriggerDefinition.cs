namespace Modeler.Api.Domain;

public sealed class TriggerDefinition : BaseEntity
{
    public string TriggerKey { get; set; } = null!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }
}
