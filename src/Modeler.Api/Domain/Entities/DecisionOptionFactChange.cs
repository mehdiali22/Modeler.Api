using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class DecisionOptionFactChange : BaseEntity
{
    public int ScenarioDecisionOptionId { get; set; }
    public int FactId { get; set; }
    public FactChangeOp Op { get; set; }

    [MaxLength(200)]
    public string? Value { get; set; }
}
