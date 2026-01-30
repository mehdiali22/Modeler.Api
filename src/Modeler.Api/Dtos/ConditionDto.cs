namespace Modeler.Api.Dtos;

public sealed class ConditionDto
{
    public int Id { get; set; }
    public string ConditionKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public string Expression { get; set; } = default!;
    public string? FailMessage { get; set; }
}
