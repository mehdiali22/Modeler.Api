namespace Modeler.Api.Dtos;

public sealed class KartablRoutingRuleDto
{
    public int Id { get; set; }
    public string RuleKey { get; set; } = default!;
    public string? OwnerSubdomain { get; set; }
    public int Priority { get; set; }

    public int? FromKartablId { get; set; }
    public int TargetKartablId { get; set; }

    /// <summary>
    /// JSON array of condition ids, مثال: [1,2,3]
    /// </summary>
    public string ConditionIdsJson { get; set; } = "[]";

    public string? TitleFa { get; set; }
    public string? Description { get; set; }
}
