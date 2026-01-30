using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class ScenarioDecision : BaseEntity
{
    public int ScenarioId { get; set; }

    [MaxLength(150)]
    public string DecisionKey { get; set; } = default!;

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    [MaxLength(150)]
    public string? UiActionKey { get; set; }
}
