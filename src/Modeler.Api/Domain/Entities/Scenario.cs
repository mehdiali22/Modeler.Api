using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class Scenario : BaseEntity
{
    [MaxLength(150)]
    public string ScenarioKey { get; set; } = default!;

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    public int StageId { get; set; }

    [MaxLength(150)]
    public string? OwnerSubdomain { get; set; }
    public int? TriggerId { get; set; }
    public TriggerDefinition? Trigger { get; set; }

}
