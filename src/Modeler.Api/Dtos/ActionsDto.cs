using Modeler.Api.Domain;

namespace Modeler.Api.Dtos;

public sealed class ActionsDto
{
    public int Id { get; set; }
    public string ActionKey { get; set; } = default!;
    public string? TitleFa { get; set; }
    public int? TargetArtifactId { get; set; }
    public ExecutorKind? ExecutorKind { get; set; }
    public int? ExecutorActorId { get; set; }
    public string? Description { get; set; }
    public string? DefaultParamsJson { get; set; }
}
