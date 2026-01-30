using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

[ApiController]
[Route("api/dictionary-terms")]
public sealed class DictionaryTermController : ControllerBase
{
    private readonly ModelerDbContext _db;

    public DictionaryTermController(ModelerDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<DictionaryTermDto>>> GetAll()
    {
        IQueryable<DictionaryTerm> q = _db.DictionaryTerms.AsNoTracking();
        

        var rows = await q.OrderBy(x => x.TermKey).ToListAsync();
        return rows.Select(Map.ToDto).ToList();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DictionaryTermDto>> GetById(int id)
    {
        var row = await _db.DictionaryTerms.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (row == null) return NotFound();
        return Map.ToDto(row);
    }

    [HttpPost]
    public async Task<ActionResult<DictionaryTermDto>> Create(DictionaryTermDto input)
    {
        // برای Create، Id رو نادیده می‌گیریم تا Identity تولید کنه
        input.Id = 0;

        var entity = Map.ToEntity(input);
        _db.DictionaryTerms.Add(entity);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, Map.ToDto(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<DictionaryTermDto>> Update(int id, DictionaryTermDto input)
    {
        if (id != input.Id) return BadRequest("id mismatch");

        var exists = await CrudHelpers.ExistsAsync<DictionaryTerm>(_db, id);
        if (!exists) return NotFound();

        var entity = Map.ToEntity(input);
        _db.Entry(entity).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        return Map.ToDto(entity);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await CrudHelpers.FindAsync<DictionaryTerm>(_db, id);
        if (row == null) return NotFound();

        _db.DictionaryTerms.Remove(row);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
