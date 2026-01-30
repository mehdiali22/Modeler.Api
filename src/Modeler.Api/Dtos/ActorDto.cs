using Modeler.Api.Domain;

namespace Modeler.Api.Dtos;

public sealed class ActorDto
{
    public int Id { get; set; }
    public string ActorKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public ExecutorKind Kind { get; set; }
    public string? Description { get; set; }
}
