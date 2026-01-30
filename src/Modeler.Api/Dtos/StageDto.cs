namespace Modeler.Api.Dtos;

public sealed class StageDto
{
    public int Id { get; set; }
    public int ProcessId { get; set; }
    public string StageKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }
    public int? Order { get; set; }
}
