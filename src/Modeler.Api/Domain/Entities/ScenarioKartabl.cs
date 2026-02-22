namespace Modeler.Api.Domain;

/// <summary>
/// Many-to-many link between Scenario and Kartabl.
/// Keeps the model clean: Scenario is "available" in one or more kartabls.
/// </summary>
public sealed class ScenarioKartabl
{
    public int ScenarioId { get; set; }
    public int KartablId { get; set; }
}
