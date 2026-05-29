using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class ActionStateTransition : BaseEntity
{
    public int? ScenarioId { get; set; }
    public int ActionId { get; set; }
    public int? FromStateId { get; set; }
    public int? ToStateId { get; set; }
    public int? DecisionId { get; set; }
    public int? DecisionOptionId { get; set; }

    [MaxLength(300)]
    public string? LabelFa { get; set; }

    public int SortOrder { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }
}
