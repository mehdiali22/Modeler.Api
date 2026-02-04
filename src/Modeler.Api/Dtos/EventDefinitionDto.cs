namespace Modeler.Api.Dtos;

public sealed class EventDefinitionDto
{
    public int Id { get; set; }
    public string EventKey { get; set; } = null!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }
}
