namespace Modeler.Api.Dtos;

public sealed class SubProcessDto
{
    public int Id { get; set; }
    public int ProcessId { get; set; }
    public string SubProcessKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }
    public int? Order { get; set; }
}
