using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class Actor : BaseEntity
{
    [MaxLength(150)]
    public string ActorKey { get; set; } = default!;

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    public ExecutorKind Kind { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }
}
