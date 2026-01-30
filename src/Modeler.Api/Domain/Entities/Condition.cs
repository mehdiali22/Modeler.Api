using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class Condition : BaseEntity
{
    [MaxLength(150)]
    public string ConditionKey { get; set; } = default!;

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    [MaxLength(4000)]
    public string Expression { get; set; } = default!;

    [MaxLength(1000)]
    public string? FailMessage { get; set; }
}
