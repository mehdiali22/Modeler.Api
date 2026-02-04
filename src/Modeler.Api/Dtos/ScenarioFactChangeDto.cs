using Modeler.Api.Domain;

namespace Modeler.Api.Dtos;

public sealed class ScenarioFactChangeDto
{
    public int Id { get; set; }
    public int ScenarioId { get; set; }
    public int FactId { get; set; }
    public FactChangeOp Op { get; set; }
    public string? Value { get; set; }
}
