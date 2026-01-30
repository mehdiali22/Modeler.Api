using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class Fact : BaseEntity
{
    public int ArtifactId { get; set; }

    [MaxLength(150)]
    public string FactKey { get; set; } = default!;

    public FactValueType ValueType { get; set; }

    [MaxLength(2000)]
    public string? Meaning { get; set; }
}
