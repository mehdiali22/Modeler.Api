using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;
using System;
using System.Linq;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/kartabls")]
public sealed class KartablController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public KartablController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<KartablDto>>> GetAll()
    {
        var rows = await _db.Kartabls
            .AsNoTracking()
            .OrderBy(x => x.KartablKey)
            .ToListAsync();

        return rows.Select(Map.ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<KartablDto>> GetById(int id)
    {
        var row = await _db.Kartabls.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();
        return Map.ToDto(row);
    }

    // ---------------- WorkItems in a Kartabl (runtime queue) ----------------
    // NOTE: Queue filtering is done using denormalized WorkItem columns (e.g., CaseStatus)
    // so we don't need JSON queries over FactsJson.

    [HttpGet("{id:int}/work-items")]
    public async Task<ActionResult<PagedResultDto<WorkItemDto>>> GetWorkItems(
        int id,
        [FromQuery] string? ownerSubdomain = null,
        // status can be a single value (e.g. ReadyForApproval) or a comma-separated list (e.g. Ready,WaitingKsr)
        [FromQuery] string? status = null,
        // referenceNo/caseId can be a single value or a comma/semicolon-separated list.
        [FromQuery] string? referenceNo = null,
        [FromQuery] string? caseId = null,
        // free-text search (use qField/qMode to control the matching)
        [FromQuery] string? q = null,
        // qMode: contains | startsWith | exact
        [FromQuery] string? qMode = "contains",
        // qField: all | key | title | referenceNo | caseId
        [FromQuery] string? qField = "all",
        [FromQuery] string? sort = "-updated",
        [FromQuery] int skip = 0,
        [FromQuery] int take = 100)
    {
        // ensure kartabl exists (better UX than returning empty list when id is wrong)
        var kartablExists = await _db.Kartabls.AsNoTracking().AnyAsync(x => x.Id == id);
        if (!kartablExists) return NotFound($"Kartabl id={id} not found.");

        if (skip < 0) skip = 0;
        if (take <= 0) take = 100;
        if (take > 500) take = 500;

        var query = _db.WorkItems
            .AsNoTracking()
            .Where(w => w.CurrentKartablId == id);

        if (!string.IsNullOrWhiteSpace(ownerSubdomain))
            query = query.Where(w => w.OwnerSubdomain == ownerSubdomain);

        if (!string.IsNullOrWhiteSpace(status))
        {
            var parts = status
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToArray();

            if (parts.Length == 1)
                query = query.Where(w => w.CaseStatus == parts[0]);
            else if (parts.Length > 1)
                query = query.Where(w => w.CaseStatus != null && parts.Contains(w.CaseStatus));
        }

        // referenceNo filter (single or multi)
        if (!string.IsNullOrWhiteSpace(referenceNo))
        {
            var refs = ParseList(referenceNo);
            if (refs.Length == 1)
                query = query.Where(w => w.ReferenceNo != null && w.ReferenceNo == refs[0]);
            else if (refs.Length > 1)
                query = query.Where(w => w.ReferenceNo != null && refs.Contains(w.ReferenceNo));
        }

        // caseId filter (single or multi)
        if (!string.IsNullOrWhiteSpace(caseId))
        {
            var ids = ParseList(caseId);
            if (ids.Length == 1)
                query = query.Where(w => w.CaseId != null && w.CaseId == ids[0]);
            else if (ids.Length > 1)
                query = query.Where(w => w.CaseId != null && ids.Contains(w.CaseId));
        }

        // free-text search
        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            qMode = (qMode ?? "contains").Trim().ToLowerInvariant();
            qField = (qField ?? "all").Trim().ToLowerInvariant();

            // We'll build the predicate based on the selected field + mode.
            // Note: SQL Server collation is typically case-insensitive, so we don't force ToLower() (keeps indexes usable).
            bool useContains = qMode is "contains" or "contain";
            bool useStartsWith = qMode is "startswith" or "starts" or "prefix";
            bool useExact = qMode is "exact" or "eq";

            if (!useContains && !useStartsWith && !useExact)
                useContains = true;

            query = qField switch
            {
                "key" => query.Where(w =>
                    useExact ? w.WorkItemKey == q :
                    useStartsWith ? w.WorkItemKey.StartsWith(q) :
                    w.WorkItemKey.Contains(q)
                ),
                "title" => query.Where(w => w.Title != null && (
                    useExact ? w.Title == q :
                    useStartsWith ? w.Title.StartsWith(q) :
                    w.Title.Contains(q)
                )),
                "referenceno" => query.Where(w => w.ReferenceNo != null && (
                    useExact ? w.ReferenceNo == q :
                    useStartsWith ? w.ReferenceNo.StartsWith(q) :
                    w.ReferenceNo.Contains(q)
                )),
                "caseid" => query.Where(w => w.CaseId != null && (
                    useExact ? w.CaseId == q :
                    useStartsWith ? w.CaseId.StartsWith(q) :
                    w.CaseId.Contains(q)
                )),
                _ => query.Where(w =>
                    // key is non-null
                    (useExact ? w.WorkItemKey == q : useStartsWith ? w.WorkItemKey.StartsWith(q) : w.WorkItemKey.Contains(q))
                    || (w.Title != null && (useExact ? w.Title == q : useStartsWith ? w.Title.StartsWith(q) : w.Title.Contains(q)))
                    || (w.ReferenceNo != null && (useExact ? w.ReferenceNo == q : useStartsWith ? w.ReferenceNo.StartsWith(q) : w.ReferenceNo.Contains(q)))
                    || (w.CaseId != null && (useExact ? w.CaseId == q : useStartsWith ? w.CaseId.StartsWith(q) : w.CaseId.Contains(q)))
                )
            };
        }

        // sorting
        //  -updated (default): newest first
        //   updated: oldest first
        //  -created / created
        //  -id / id
        //  -key / key
        //  -title / title
        sort = (sort ?? "-updated").Trim().ToLowerInvariant();

        IOrderedQueryable<WorkItem> ordered = sort switch
        {
            "updated" => query.OrderBy(w => w.UpdatedAtUtc).ThenBy(w => w.Id),
            "-created" => query.OrderByDescending(w => w.CreatedAtUtc).ThenByDescending(w => w.Id),
            "created" => query.OrderBy(w => w.CreatedAtUtc).ThenBy(w => w.Id),
            "-id" => query.OrderByDescending(w => w.Id),
            "id" => query.OrderBy(w => w.Id),
            "-key" => query.OrderByDescending(w => w.WorkItemKey).ThenByDescending(w => w.Id),
            "key" => query.OrderBy(w => w.WorkItemKey).ThenBy(w => w.Id),
            "-title" => query.OrderByDescending(w => w.Title ?? "").ThenByDescending(w => w.Id),
            "title" => query.OrderBy(w => w.Title ?? "").ThenBy(w => w.Id),
            _ => query.OrderByDescending(w => w.UpdatedAtUtc).ThenByDescending(w => w.Id)
        };

        var total = await query.CountAsync();

        var rows = await ordered
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return Ok(new PagedResultDto<WorkItemDto>
        {
            Total = total,
            Items = rows.Select(Map.ToDto).ToList()
        });
    }

    private static string[] ParseList(string raw)
    {
        return raw
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    [HttpPost]
    public async Task<ActionResult<KartablDto>> Create([FromBody] KartablDto input)
    {
        input.Id = 0;
        var entity = Map.ToEntity(input);

        _db.Kartabls.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, Map.ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<KartablDto>> Update(int id, [FromBody] KartablDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var exists = await CrudHelpers.ExistsAsync<Kartabl>(_db, id);
        if (!exists) return NotFound();

        var entity = Map.ToEntity(input);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return Map.ToDto(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await CrudHelpers.FindAsync<Kartabl>(_db, id);
        if (row == null) return NotFound();

        _db.Kartabls.Remove(row);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
