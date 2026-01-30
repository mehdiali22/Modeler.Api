using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class ScenarioInputArtifact : BaseEntity
{
    public int ScenarioId { get; set; }
    public int ArtifactId { get; set; }

    [MaxLength(150)]
    public string? RoleKey { get; set; }
}
