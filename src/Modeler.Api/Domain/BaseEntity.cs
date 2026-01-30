namespace Modeler.Api.Domain;

public abstract class BaseEntity
{
    public int Id { get; set; } // INT (Identity)
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}
