namespace Modeler.Api.Dtos;

public sealed class ArtifactDto
{
    public int Id { get; set; }
    public string ArtifactKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }
    public bool IsChildOfCase { get; set; }
}
