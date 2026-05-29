using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class EntityStatee : BaseEntity
{
    public int ArtifactId { get; set; }

    [MaxLength(150)]
    public string StateKey { get; set; } = default!;

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    public string ConditionJson { get; set; } = "[]";

    [MaxLength(1000)]
    public string? Description { get; set; }
}
