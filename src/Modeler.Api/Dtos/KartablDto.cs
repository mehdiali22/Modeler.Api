namespace Modeler.Api.Dtos;

public sealed class KartablDto
{
    public int Id { get; set; }
    public string KartablKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public string? Description { get; set; }
    public string? OwnerSubdomain { get; set; }
}
