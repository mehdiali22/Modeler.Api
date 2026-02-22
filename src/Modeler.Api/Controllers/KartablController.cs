using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Dtos;
using Modeler.Api.Mappers;
using Modeler.Api.Persistence;

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
