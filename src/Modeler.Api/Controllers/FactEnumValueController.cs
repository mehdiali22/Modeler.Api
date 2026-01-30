using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/fact-enum-values")]
public sealed class FactEnumValueController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public FactEnumValueController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<FactEnumValueDto>>> GetAll([FromQuery] int? factId)
    {
        IQueryable<FactEnumValue> q = _db.FactEnumValues.AsNoTracking();
        
        if (factId.HasValue) q = q.Where(x => x.FactId == factId.Value);

        var rows = await q.OrderBy(x => x.EnumKey).ToListAsync();
        return rows.Select(Map.ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FactEnumValueDto>> GetById(int id)
    {
        var row = await _db.FactEnumValues.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();
        return Map.ToDto(row);
    }

    [HttpPost]
    public async Task<ActionResult<FactEnumValueDto>> Create(FactEnumValueDto input)
    {
        // برای Create، Id رو نادیده می‌گیریم تا Identity تولید کنه
        input.Id = 0;

        var entity = Map.ToEntity(input);
        _db.FactEnumValues.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, Map.ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<FactEnumValueDto>> Update(int id, FactEnumValueDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var exists = await CrudHelpers.ExistsAsync<FactEnumValue>(_db, id);
        if (!exists) return NotFound();

        var entity = Map.ToEntity(input);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return Map.ToDto(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await CrudHelpers.FindAsync<FactEnumValue>(_db, id);
        if (row == null) return NotFound();

        _db.FactEnumValues.Remove(row);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
