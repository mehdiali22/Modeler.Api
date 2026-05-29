namespace Modeler.Api.Dtos;

public sealed class WorkItemDto
{
    public int Id { get; set; }
    public string WorkItemKey { get; set; } = default!;
    public string OwnerSubdomain { get; set; } = default!;

    public string? ReferenceNo { get; set; }
    public string? CaseId { get; set; }

    public int? CurrentKartablId { get; set; }
    public string? FactsJson { get; set; }
    public string? CaseStatus { get; set; }
    public string? Title { get; set; }
}
