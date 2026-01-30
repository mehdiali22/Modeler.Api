using Microsoft.EntityFrameworkCore;
using Modeler.Api.Domain;
using Modeler.Api.Persistence;

namespace Modeler.Api.Controllers;

internal static class CrudHelpers
{
    public static async Task<bool> ExistsAsync<TEntity>(ModelerDbContext db, int id) where TEntity : BaseEntity
        => await db.Set<TEntity>().AnyAsync(x => x.Id == id);

    public static async Task<TEntity?> FindAsync<TEntity>(ModelerDbContext db, int id) where TEntity : BaseEntity
        => await db.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
}
