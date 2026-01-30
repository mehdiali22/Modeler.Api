using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class SubProcess : BaseEntity
{
    public int ProcessId { get; set; }

    [MaxLength(150)]
    public string SubProcessKey { get; set; } = default!;

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    public int? Order { get; set; }
}
