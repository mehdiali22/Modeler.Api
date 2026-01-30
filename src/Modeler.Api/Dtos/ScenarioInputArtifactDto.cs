namespace Modeler.Api.Dtos;

public sealed class ScenarioInputArtifactDto
{
    public int Id { get; set; }
    public int ScenarioId { get; set; }
    public int ArtifactId { get; set; }
    public string? RoleKey { get; set; }
}
