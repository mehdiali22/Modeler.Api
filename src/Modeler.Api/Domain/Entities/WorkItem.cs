namespace Modeler.Api.Domain;

/// <summary>
/// A lightweight runtime queue item ("work item") that sits in a Kartabl.
/// This is NOT part of the modeling definitions (Process/Stage/Scenario). It is
/// the minimal persistence needed to track where an instance currently is.
/// </summary>
public sealed class WorkItem : BaseEntity
{
    public required string WorkItemKey { get; set; }
    public required string OwnerSubdomain { get; set; }

    /// <summary>
    /// A searchable external reference number for the work item (e.g. CaseNo/AdmId).
    /// This is denormalized (typically mirrors FactsJson["ReferenceNo"]).
    /// </summary>
    public string? ReferenceNo { get; set; }

    /// <summary>
    /// A searchable external identifier for the work item (string to support numeric/non-numeric ids).
    /// This is denormalized (typically mirrors FactsJson["CaseId"]).
    /// </summary>
    public string? CaseId { get; set; }

    public int? CurrentKartablId { get; set; }
    public Kartabl? CurrentKartabl { get; set; }

    /// <summary>
    /// Optional snapshot of facts as JSON (FactKey -> value) for debugging / simple runtime.
    /// </summary>
    public string? FactsJson { get; set; }

    /// <summary>
    /// A denormalized, query-friendly status value (typically mirrors FactsJson["CaseStatus"]).
    /// This exists so kartabl queues can be filtered/sorted efficiently without JSON queries.
    /// </summary>
    public string? CaseStatus { get; set; }

    public string? Title { get; set; }
}
