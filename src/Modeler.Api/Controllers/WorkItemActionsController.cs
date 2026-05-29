using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/work-item-actions")]
public sealed class WorkItemActionsController : ControllerBase
{
    private static readonly HashSet<string> AllowedStatuses = new(StringComparer.OrdinalIgnoreCase)
    {
        "Pending", "Done", "Failed"
    };

    private readonly ModelerDbContext _db;

    public WorkItemActionsController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet("pending")]
    public async Task<ActionResult<List<WorkItemActionDto>>> GetPending(
        [FromQuery] int take = 100,
        [FromQuery] string? source = null,
        [FromQuery] int? actionId = null)
    {
        take = Math.Clamp(take, 1, 500);

        var query = _db.WorkItemActions
            .AsNoTracking()
            .Where(x => x.Status == "Pending");

        if (!string.IsNullOrWhiteSpace(source))
            query = query.Where(x => x.Source == source.Trim());

        if (actionId.HasValue)
            query = query.Where(x => x.ActionId == actionId.Value);

        var rows = await query
            .OrderBy(x => x.CreatedAtUtc)
            .ThenBy(x => x.Id)
            .Take(take)
            .ToListAsync();

        return rows.Select(Map.ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WorkItemActionDto>> GetById(int id)
    {
        var row = await _db.WorkItemActions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound($"WorkItemAction not found: {id}");
        return Map.ToDto(row);
    }

    [HttpPost("{id:int}/mark-done")]
    public async Task<ActionResult<WorkItemActionDto>> MarkDone(int id)
    {
        var row = await _db.WorkItemActions.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound($"WorkItemAction not found: {id}");

        row.Status = "Done";
        row.CompletedAtUtc = DateTime.UtcNow;
        row.FailedAtUtc = null;
        row.LastError = null;
        row.LastAttemptAtUtc = DateTime.UtcNow;
        row.AttemptCount += 1;

        await _db.SaveChangesAsync();
        return Map.ToDto(row);
    }

    [HttpPost("{id:int}/mark-failed")]
    public async Task<ActionResult<WorkItemActionDto>> MarkFailed(int id, [FromBody] MarkWorkItemActionFailedDto? req)
    {
        var row = await _db.WorkItemActions.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound($"WorkItemAction not found: {id}");

        row.Status = "Failed";
        row.FailedAtUtc = DateTime.UtcNow;
        row.CompletedAtUtc = null;
        row.LastError = string.IsNullOrWhiteSpace(req?.Error) ? "Action failed." : req!.Error!.Trim();
        row.LastAttemptAtUtc = DateTime.UtcNow;
        row.AttemptCount += 1;

        await _db.SaveChangesAsync();
        return Map.ToDto(row);
    }

    [HttpPost("{id:int}/retry")]
    public async Task<ActionResult<WorkItemActionDto>> Retry(int id)
    {
        var row = await _db.WorkItemActions.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound($"WorkItemAction not found: {id}");

        if (row.Status != "Failed")
            return BadRequest("Only Failed actions can be moved back to Pending for retry.");

        row.Status = "Pending";
        row.FailedAtUtc = null;
        row.CompletedAtUtc = null;
        row.LastError = null;

        await _db.SaveChangesAsync();
        return Map.ToDto(row);
    }

    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<WorkItemActionDto>> SetStatus(int id, [FromQuery] string status, [FromBody] MarkWorkItemActionFailedDto? req = null)
    {
        if (string.IsNullOrWhiteSpace(status) || !AllowedStatuses.Contains(status.Trim()))
            return BadRequest("status must be one of: Pending, Done, Failed.");

        var row = await _db.WorkItemActions.FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound($"WorkItemAction not found: {id}");

        var normalized = NormalizeStatus(status);
        row.Status = normalized;
        row.LastAttemptAtUtc = DateTime.UtcNow;

        if (normalized == "Done")
        {
            row.CompletedAtUtc = DateTime.UtcNow;
            row.FailedAtUtc = null;
            row.LastError = null;
            row.AttemptCount += 1;
        }
        else if (normalized == "Failed")
        {
            row.FailedAtUtc = DateTime.UtcNow;
            row.CompletedAtUtc = null;
            row.LastError = string.IsNullOrWhiteSpace(req?.Error) ? "Action failed." : req!.Error!.Trim();
            row.AttemptCount += 1;
        }
        else
        {
            row.CompletedAtUtc = null;
            row.FailedAtUtc = null;
            row.LastError = null;
        }

        await _db.SaveChangesAsync();
        return Map.ToDto(row);
    }

    private static string NormalizeStatus(string status)
    {
        var s = status.Trim();
        if (s.Equals("pending", StringComparison.OrdinalIgnoreCase)) return "Pending";
        if (s.Equals("done", StringComparison.OrdinalIgnoreCase)) return "Done";
        if (s.Equals("failed", StringComparison.OrdinalIgnoreCase)) return "Failed";
        return s;
    }
}
