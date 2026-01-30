namespace Modeler.Api.Dtos;

public sealed class ScenarioDto
{
    public int Id { get; set; }
    public string ScenarioKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }
    public int StageId { get; set; }
    public string? OwnerSubdomain { get; set; }
}
