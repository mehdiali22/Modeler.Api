namespace Modeler.Api.Dtos;

public sealed class FactEnumValueDto
{
    public int Id { get; set; }
    public int FactId { get; set; }
    public string EnumKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public string? Value { get; set; }
}
