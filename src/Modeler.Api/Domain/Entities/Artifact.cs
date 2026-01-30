using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class Artifact : BaseEntity
{
    [MaxLength(150)]
    public string ArtifactKey { get; set; } = default!;

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    public bool IsChildOfCase { get; set; }
}
