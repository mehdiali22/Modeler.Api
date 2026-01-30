namespace Modeler.Api.Dtos;

public sealed class ProcessDto
{
    public int Id { get; set; }
    public string ProcessKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }
    public int? Order { get; set; }
}
