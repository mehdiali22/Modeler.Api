using Modeler.Api.Domain;

namespace Modeler.Api.Dtos;

public sealed class DecisionOptionFactChangeDto
{
    public int Id { get; set; }
    public int ScenarioDecisionOptionId { get; set; }
    public int FactId { get; set; }
    public FactChangeOp Op { get; set; }
    public string? Value { get; set; }
}
