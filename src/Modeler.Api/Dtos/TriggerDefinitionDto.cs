namespace Modeler.Api.Dtos;

public sealed class TriggerDefinitionDto
{
    public int Id { get; set; }
    public string TriggerKey { get; set; } = null!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }
}
