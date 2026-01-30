using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/action-catalog")]
public sealed class ActionCatalogController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public ActionCatalogController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ActionCatalogDto>>> GetAll()
    {
        IQueryable<ActionCatalog> q = _db.ActionCatalog.AsNoTracking();
        

        var rows = await q.OrderBy(x => x.ActionKey).ToListAsync();
        return rows.Select(Map.ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ActionCatalogDto>> GetById(int id)
    {
        var row = await _db.ActionCatalog.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();
        return Map.ToDto(row);
    }

    [HttpPost]
    public async Task<ActionResult<ActionCatalogDto>> Create(ActionCatalogDto input)
    {
        // برای Create، Id رو نادیده می‌گیریم تا Identity تولید کنه
        input.Id = 0;

        var entity = Map.ToEntity(input);
        _db.ActionCatalog.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, Map.ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ActionCatalogDto>> Update(int id, ActionCatalogDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var exists = await CrudHelpers.ExistsAsync<ActionCatalog>(_db, id);
        if (!exists) return NotFound();

        var entity = Map.ToEntity(input);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return Map.ToDto(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await CrudHelpers.FindAsync<ActionCatalog>(_db, id);
        if (row == null) return NotFound();

        _db.ActionCatalog.Remove(row);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
