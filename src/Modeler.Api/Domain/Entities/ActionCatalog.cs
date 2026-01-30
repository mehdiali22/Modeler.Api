using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class ActionCatalog : BaseEntity
{
    [MaxLength(150)]
    public string ActionKey { get; set; } = default!;

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    public int? TargetArtifactId { get; set; }

    public ExecutorKind? ExecutorKind { get; set; }
    public int? ExecutorActorId { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    public string? DefaultParamsJson { get; set; }
}
