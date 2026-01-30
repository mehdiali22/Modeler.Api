using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class FactEnumValue : BaseEntity
{
    public int FactId { get; set; }

    [MaxLength(150)]
    public string EnumKey { get; set; } = default!;

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    [MaxLength(200)]
    public string? Value { get; set; }
}
