namespace Modeler.Api.Dtos;

public sealed class DictionaryTermDto
{
    public int Id { get; set; }
    public string TermKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }
}
