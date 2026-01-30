using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class ScenarioFactChange : BaseEntity
{
    public int ScenarioId { get; set; }
    public int FactId { get; set; }
    public FactChangeOp Op { get; set; }

    [MaxLength(200)]
    public string? Value { get; set; }
}
