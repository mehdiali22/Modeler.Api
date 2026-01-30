using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class Stage : BaseEntity
{
    public int ProcessId { get; set; }

    [MaxLength(150)]
    public string StageKey { get; set; } = default!;

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    public int? Order { get; set; }
}
