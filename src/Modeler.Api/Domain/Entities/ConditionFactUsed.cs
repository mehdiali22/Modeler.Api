namespace Modeler.Api.Domain;

/// <summary>
/// many-to-many بین Condition و Fact (factsUsed)
/// </summary>
public sealed class ConditionFactUsed
{
    public int ConditionId { get; set; }
    public int FactId { get; set; }
}
