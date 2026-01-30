using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/sub-processes")]
public sealed class SubProcessController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public SubProcessController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<SubProcessDto>>> GetAll([FromQuery] int? processId)
    {
        IQueryable<SubProcess> q = _db.SubProcesses.AsNoTracking();
        
        if (processId.HasValue) q = q.Where(x => x.ProcessId == processId.Value);

        var rows = await q.OrderBy(x => x.SubProcessKey).ToListAsync();
        return rows.Select(Map.ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SubProcessDto>> GetById(int id)
    {
        var row = await _db.SubProcesses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();
        return Map.ToDto(row);
    }

    [HttpPost]
    public async Task<ActionResult<SubProcessDto>> Create(SubProcessDto input)
    {
        // برای Create، Id رو نادیده می‌گیریم تا Identity تولید کنه
        input.Id = 0;

        var entity = Map.ToEntity(input);
        _db.SubProcesses.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, Map.ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SubProcessDto>> Update(int id, SubProcessDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var exists = await CrudHelpers.ExistsAsync<SubProcess>(_db, id);
        if (!exists) return NotFound();

        var entity = Map.ToEntity(input);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return Map.ToDto(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await CrudHelpers.FindAsync<SubProcess>(_db, id);
        if (row == null) return NotFound();

        _db.SubProcesses.Remove(row);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
