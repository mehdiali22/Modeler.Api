using Modeler.Api.Domain;

namespace Modeler.Api.Dtos;

public sealed class FactDto
{
    public int Id { get; set; }
    public int ArtifactId { get; set; }
    public string FactKey { get; set; } = default!;
    public FactValueType ValueType { get; set; }
    public string? Meaning { get; set; }
}
