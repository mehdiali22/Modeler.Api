using System.ComponentModel.DataAnnotations;

namespace Modeler.Api.Domain;

public sealed class ScenarioDecisionOption : BaseEntity
{
    public int ScenarioDecisionId { get; set; }

    [MaxLength(150)]
    public string OptionKey { get; set; } = default!;

    [MaxLength(300)]
    public string? TitleFa { get; set; }

    // چون جدول OptionCondition/OptionAction تو لیست نبود،
    // این‌ها رو JSON نگه می‌داریم تا UI رو پوشش بده.
    public string? ConditionIdsJson { get; set; } // [int,...]
    public string? ActionIdsJson { get; set; } // [int,...]
    public string? ProducedEventIdsJson { get; set; } // [int,...]
}
